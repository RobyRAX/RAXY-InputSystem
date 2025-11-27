using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace RAXY.InputSystem
{
    public class GestureInput_NewInputSystem : GestureInputBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            EnhancedTouch.EnhancedTouchSupport.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EnhancedTouch.EnhancedTouchSupport.Disable();
        }

        private readonly List<int> endedFingerIdsBuffer = new();

        protected override void CheckTouches()
        {
            if (!EnhancedTouch.EnhancedTouchSupport.enabled)
                return;

            var touches = EnhancedTouch.Touch.activeTouches;

            // Track ended/canceled touches for removal
            endedFingerIdsBuffer.Clear();

            foreach (var et in touches)
            {
                int fingerId = et.finger.index;
                Vector2 pos = et.screenPosition;

                bool isActive = _activeTouches.ContainsKey(fingerId);
                bool isIgnored = _ignoredTouches.ContainsKey(fingerId);

                if (isActive || isIgnored)
                {
                    var dict = isActive ? _activeTouches : _ignoredTouches;
                    var touchData = dict[fingerId];

                    // If ended or canceled, mark for removal
                    if (et.phase == TouchPhase.Ended ||
                        et.phase == TouchPhase.Canceled)
                    {
                        endedFingerIdsBuffer.Add(fingerId);
                    }
                    else
                    {
                        // ✅ FIX: properly update touch delta, position, and phase
                        touchData.UpdateNewTouch(et);
                    }
                }
                else
                {
                    // New touch begins
                    if (et.phase != TouchPhase.Ended &&
                        et.phase != TouchPhase.Canceled)
                    {
                        bool isOverUI = IsTouchOverUI(pos);
                        var targetDict = isOverUI ? _ignoredTouches : _activeTouches;
                        targetDict[fingerId] = new TouchData(et);
                    }
                }
            }

            // Remove ended/canceled touches
            foreach (int id in endedFingerIdsBuffer)
            {
                _activeTouches.Remove(id);
                _ignoredTouches.Remove(id);
            }

            // If there are no touches active, clear for consistency
            if (touches.Count == 0)
            {
                _activeTouches.Clear();
                _ignoredTouches.Clear();
            }
        }

        protected override bool IsTouchOverUI(Vector2 touchPos)
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData eventData = new(EventSystem.current)
            {
                position = touchPos
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
