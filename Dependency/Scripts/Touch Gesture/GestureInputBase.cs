using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RAXY.InputSystem
{
    public abstract class GestureInputBase : MonoBehaviour
    {
        public const string SWIPE = "swipe";
        public const string PINCH = "pinch";
        public event Action OnNoTouch;

        [TitleGroup("Setting")]
        public bool invertSwipeAxisY = true;

        [TitleGroup("Setting")]
        public bool invertPinch = true;

        [TitleGroup("Setting")]
        public float swipeSensitivity = 1;

        [TitleGroup("Setting")]
        public float pinchSensitivity = 1;

        [TitleGroup("Event")]
        public InputActionEventSO swipeEvent;
        public InputActionEventSO pinchEvent;

        [TitleGroup("Swipe")]
        [ShowInInspector, ReadOnly]
        public Vector2 SwipeDelta
        {
            get
            {
                if (_activeTouches == null || _activeTouches.Count == 0)
                    return Vector2.zero;

                if (_activeTouches.Count > 1)
                    return Vector2.zero;

                Vector2 delta = _activeTouches.Values.First().Delta * swipeSensitivity;

                if (invertSwipeAxisY)
                    delta.y *= -1f;

                return delta;
            }
        }

        [TitleGroup("Debug")]
        [SerializeField]
        float _pinchNewDist;

        [TitleGroup("Debug")]
        [SerializeField]
        float _pinchDistanceDelta;

        [TitleGroup("Debug")]
        [SerializeField]
        float _pinchLastDistance;

        [TitleGroup("Pinch")]
        [ShowInInspector, ReadOnly]
        public float PinchDelta { get; protected set; }

        void HandlePinchDelta()
        {
            if (IsPinching == false || _activeTouches.Count < 2)
            {
                PinchDelta = 0;
                return;
            }

            var touches = _activeTouches.Values.Take(2).ToArray();
            float currentDist = Vector2.Distance(touches[0].CurrentPos, touches[1].CurrentPos);

            // If this is the FIRST frame of pinching, initialize the last distance
            if (_pinchLastDistance == 0)
            {
                _pinchLastDistance = currentDist;
                PinchDelta = 0;
                return;
            }

            _pinchNewDist = currentDist;
            _pinchDistanceDelta = _pinchLastDistance - _pinchNewDist;
            _pinchLastDistance = _pinchNewDist;

            float final = _pinchDistanceDelta * pinchSensitivity * (invertPinch ? 1 : -1);
            Debug.LogWarning($"{_pinchNewDist} - {_pinchDistanceDelta} - {_pinchLastDistance} - {final}");

            PinchDelta = final;
        }

        public Vector2 PinchDeltaVector2 => new Vector2(0, PinchDelta);

        [TitleGroup("Touches")]
        [ShowInInspector]
        protected Dictionary<int, TouchData> _activeTouches = new();

        [TitleGroup("Touches")]
        [ShowInInspector]
        protected Dictionary<int, TouchData> _ignoredTouches = new();

        [TitleGroup("Touches")]
        [ShowInInspector]
        bool _noTouch;
        public bool NoTouch
        {
            get => _noTouch;
            set
            {
                if (value == _noTouch)
                    return;

                _noTouch = value;

                if (_noTouch)
                {
                    OnNoTouch?.Invoke();
                    swipeEvent?.Raise(SWIPE, Vector2.zero);
                    pinchEvent?.Raise(PINCH, Vector2.zero);
                }
            }
        }

        [TitleGroup("Touches")]
        [ShowInInspector]
        bool _isSwiping;
        public bool IsSwiping
        {
            get => _isSwiping;
            set
            {
                if (_isSwiping == value)
                    return;

                _isSwiping = value;
            }
        }

        [TitleGroup("Touches")]
        [ShowInInspector]
        bool _isPinching;
        public bool IsPinching
        {
            get => _isPinching;
            set
            {
                if (_isPinching == value)
                    return;

                _isPinching = value;

                if (_isPinching)
                {
                    var touches = _activeTouches.Values.Take(2).ToArray();
                    _pinchLastDistance  = Vector2.Distance(touches[0].CurrentPos, touches[1].CurrentPos);
                }
            }
        }

        protected virtual void OnEnable()
        {
            _activeTouches?.Clear();
            _ignoredTouches?.Clear();
        }

        protected virtual void OnDisable()
        {
            _activeTouches?.Clear();
            _ignoredTouches?.Clear();
        }

        protected virtual void Update()
        {
            CheckTouches();

            NoTouch = _activeTouches.Count == 0;
            IsSwiping = _activeTouches.Count == 1;
            IsPinching = _activeTouches.Count >= 2;

            if (IsSwiping)
            {
                swipeEvent?.Raise(SWIPE, SwipeDelta);
            }
            else if (IsPinching)
            {
                HandlePinchDelta();

                if (PinchDelta != 0)
                    pinchEvent?.Raise(PINCH, PinchDeltaVector2);
            }
        }

        protected abstract void CheckTouches();
        protected abstract bool IsTouchOverUI(Vector2 touchPos);

#if UNITY_EDITOR
        public void OnGUI()
        {
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16
            };

            // --- First Window: Active Touches ---
            GUILayout.BeginArea(new Rect(Screen.width - 700, 10, 340, Screen.height - 20));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Active Touches", titleStyle);

            if (_activeTouches == null || _activeTouches.Count == 0)
            {
                GUILayout.Label("No active touches available", labelStyle);
            }
            else
            {
                foreach (var touchData in _activeTouches)
                {
                    var touch = touchData.Value;
                    if (touch == null) 
                        continue;

                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label($"ID: {touch.ID}", labelStyle);
                    GUILayout.Label($"Start Pos: {touch.StartPos}", labelStyle);
                    GUILayout.Label($"Current Pos: {touch.CurrentPos}", labelStyle);
                    GUILayout.Label($"Delta: {touch.Delta}", labelStyle);
                    GUILayout.Label($"Phase: {touch.Phase}", labelStyle);
                    GUILayout.EndVertical();

                    GUILayout.Space(10);
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            // --- Second Window: Ignored Touches ---
            GUILayout.BeginArea(new Rect(Screen.width - 350, 10, 340, Screen.height - 20));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Ignored Touches", titleStyle);

            if (_ignoredTouches == null || _ignoredTouches.Count == 0)
            {
                GUILayout.Label("No ignored touches", labelStyle);
            }
            else
            {
                foreach (var touchData in _ignoredTouches)
                {
                    var touch = touchData.Value;
                    if (touch == null) 
                        continue;

                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label($"ID: {touch.ID}", labelStyle);
                    GUILayout.Label($"Start Pos: {touch.StartPos}", labelStyle);
                    GUILayout.Label($"Current Pos: {touch.CurrentPos}", labelStyle);
                    GUILayout.Label($"Delta: {touch.Delta}", labelStyle);
                    GUILayout.Label($"Phase: {touch.Phase}", labelStyle);
                    GUILayout.EndVertical();

                    GUILayout.Space(10);
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            // --- Third Window: Touch States ---
            GUILayout.BeginArea(new Rect(Screen.width - 1050, 10, 340, 150)); // Positioned next to the other windows
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Touch States", titleStyle);

            GUILayout.Label($"No Touch: {_noTouch}", labelStyle);
            GUILayout.Label($"Is Swiping: {_isSwiping}", labelStyle);
            GUILayout.Label($"Is Pinching: {_isPinching}", labelStyle);

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
#endif
    }
}
