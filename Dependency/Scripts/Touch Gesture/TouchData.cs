using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.Touch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace RAXY.InputSystem
{
    [Serializable]
    public class TouchData
    {
        [HideInInspector] public Touch? oldTouch;
        [HideInInspector] public EnhancedTouch.Touch? newTouch;

        [ShowInInspector] public int ID { get; private set; }
        [ShowInInspector] public Vector2 StartPos { get; private set; }
        [ShowInInspector] public Vector2 CurrentPos { get; private set; }
        [ShowInInspector] public Vector2 Delta { get; private set; }
        [ShowInInspector] public TouchPhase Phase { get; private set; }

        #region --- Constructors ---

        // Old Input System constructor
        public TouchData(Touch touch)
        {
            oldTouch = touch;
            ID = touch.fingerId;
            StartPos = touch.position;
            UpdateOldTouch(touch);
        }

        // New Input System constructor
        public TouchData(EnhancedTouch.Touch touch)
        {
            newTouch = touch;
            ID = touch.finger.index;
            StartPos = touch.startScreenPosition;
            UpdateNewTouch(touch);
        }

        #endregion

        #region --- Update Methods ---

        /// <summary>
        /// Update this touch data from an old input system Touch.
        /// Keeps StartPos intact.
        /// </summary>
        public void UpdateOldTouch(Touch touch)
        {
            oldTouch = touch;
            CurrentPos = touch.position;
            Delta = touch.deltaPosition;
            Phase = touch.phase;
        }

        /// <summary>
        /// Update this touch data from a new input system EnhancedTouch.
        /// Keeps StartPos intact.
        /// </summary>
        public void UpdateNewTouch(EnhancedTouch.Touch touch)
        {
            newTouch = touch;
            CurrentPos = touch.screenPosition;
            Delta = touch.delta;

            // Convert InputSystem.TouchPhase to UnityEngine.TouchPhase
            Phase = ConvertPhase(touch.phase);
        }

        #endregion

        #region --- Utilities ---

        /// <summary>
        /// Converts InputSystem.TouchPhase to the legacy UnityEngine.TouchPhase.
        /// </summary>
        private static TouchPhase ConvertPhase(UnityEngine.InputSystem.TouchPhase phase)
        {
            switch (phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began: return TouchPhase.Began;
                case UnityEngine.InputSystem.TouchPhase.Moved: return TouchPhase.Moved;
                case UnityEngine.InputSystem.TouchPhase.Stationary: return TouchPhase.Stationary;
                case UnityEngine.InputSystem.TouchPhase.Ended: return TouchPhase.Ended;
                case UnityEngine.InputSystem.TouchPhase.Canceled: return TouchPhase.Canceled;
                default: return TouchPhase.Canceled;
            }
        }

        /// <summary>
        /// Unified update helper if you need to copy values from another TouchData.
        /// </summary>
        public void UpdateFrom(TouchData source)
        {
            if (source == null)
                return;

            CurrentPos = source.CurrentPos;
            Delta = source.Delta;
            Phase = source.Phase;
        }

        #endregion
    }
}
