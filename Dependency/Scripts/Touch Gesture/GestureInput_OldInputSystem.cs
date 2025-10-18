using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RAXY.InputSystem
{
    public class GestureInput_OldInputSystem : GestureInputBase
    {
        private readonly List<int> _endedFingerIdsBuffer = new();

        protected override void CheckTouches()
        {
            // --- Gather active touches ---
            if (Input.touchCount > 0)
            {
                _endedFingerIdsBuffer.Clear();

                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touchTemp = Input.GetTouch(i);
                    int fingerId = touchTemp.fingerId;

                    bool isActive = _activeTouches.ContainsKey(fingerId);
                    bool isIgnored = _ignoredTouches.ContainsKey(fingerId);

                    // --- Existing tracked touch ---
                    if (isActive || isIgnored)
                    {
                        var dict = isActive ? _activeTouches : _ignoredTouches;
                        var touchData = dict[fingerId];

                        // Ended / canceled
                        if (touchTemp.phase == TouchPhase.Ended || touchTemp.phase == TouchPhase.Canceled)
                        {
                            _endedFingerIdsBuffer.Add(fingerId);
                        }
                        else
                        {
                            // ✅ Update current touch data
                            touchData.UpdateOldTouch(touchTemp);
                        }
                    }
                    // --- New touch begins ---
                    else if (touchTemp.phase != TouchPhase.Ended && touchTemp.phase != TouchPhase.Canceled)
                    {
                        bool isOverUI = IsTouchOverUI(touchTemp.position);
                        var targetDict = isOverUI ? _ignoredTouches : _activeTouches;
                        targetDict[fingerId] = new TouchData(touchTemp);
                    }
                }

                // --- Remove ended/canceled touches ---
                foreach (int id in _endedFingerIdsBuffer)
                {
                    _activeTouches.Remove(id);
                    _ignoredTouches.Remove(id);
                }
            }
            else
            {
                // No touches — clear all
                _activeTouches.Clear();
                _ignoredTouches.Clear();
            }
        }

        protected override bool IsTouchOverUI(Vector2 touchPos)
        {
            if (EventSystem.current == null)
                return false;

            var inputModule = EventSystem.current.currentInputModule;

            // Works properly with old input system finger IDs
            if (inputModule is StandaloneInputModule)
                return EventSystem.current.IsPointerOverGameObject(-1);

            // Fallback raycast for edge cases
            PointerEventData eventData = new(EventSystem.current) { position = touchPos };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
