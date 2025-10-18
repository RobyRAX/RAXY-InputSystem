using System;
using UnityEngine;

namespace RAXY.InputSystem
{
    public class SanityCheck : MonoBehaviour
    {
        public InputActionEventSO eventToSubscribe;

        // Keep the reference so we can unsubscribe later
        private Action<InputContext> _onInputTriggered;

        void Start()
        {
            _onInputTriggered = (ctx) =>
            {
                Debug.Log(
                    $"🟢 <b>Input Event Triggered</b>\n" +
                    $"• Action: <color=#4FC3F7>{ctx.ActionName}</color>\n" +
                    $"• Bool: <color=#81C784>{ctx.BoolValue}</color>\n" +
                    $"• Vector2: <color=#F06292>{ctx.Vector2Value}</color>"
                );
            };

            eventToSubscribe.Subscribe(_onInputTriggered);
        }

        void OnDestroy()
        {
            if (eventToSubscribe != null)
                eventToSubscribe.Unsubscribe(_onInputTriggered);
        }
    }
}
