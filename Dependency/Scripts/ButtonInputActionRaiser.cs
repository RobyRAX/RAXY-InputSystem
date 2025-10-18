using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RAXY.InputSystem
{
    public class ButtonInputActionRaiser : InputActionRaiserBase, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Called when the pointer (finger/mouse) is pressed down on the button.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            Trigger_Press();
        }

        /// <summary>
        /// Called when the pointer (finger/mouse) is released from the button.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            Trigger_Release();
        }

        void Trigger_Press()
        {
            foreach (var actionEvent in inputActionEvents)
            {
                actionEvent.Raise(actionName, true);
                if (debug)
                    Debug.Log($"⬇️ [Pressed] {actionEvent.name} pressed from {gameObject.name}", this);
            }
        }

        void Trigger_Release()
        {
            foreach (var actionEvent in inputActionEvents)
            {
                actionEvent.Raise(actionName, false);
                if (debug)
                    Debug.Log($"⬆️ [Released] {actionEvent.name} released from {gameObject.name}", this);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Test Press")]
        private void TestPress()
        {
            foreach (var actionEvent in inputActionEvents)
                actionEvent.Raise(true);

            Debug.Log($"🧩 <b>OnScreen Button Test</b> → Pressed {inputActionEvents.Count} InputActionEvent(s)");
        }

        [ContextMenu("Test Release")]
        private void TestRelease()
        {
            foreach (var actionEvent in inputActionEvents)
                actionEvent.Raise(false);

            Debug.Log($"🧩 <b>OnScreen Button Test</b> → Released {inputActionEvents.Count} InputActionEvent(s)");
        }
#endif
    }
}
