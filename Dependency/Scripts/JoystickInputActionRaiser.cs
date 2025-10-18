using UnityEngine;
using UnityEngine.EventSystems;

namespace RAXY.InputSystem
{
    [RequireComponent(typeof(Joystick))]
    public class JoystickInputActionRaiser : InputActionRaiserBase
    {
        Joystick _joystick;

        bool _isPressed;
        public bool IsPressed
        {
            get => _isPressed;
            set
            {
                if (value == _isPressed)
                    return;

                _isPressed = value;

                if (_isPressed == false)
                {
                    Trigger_ValueChanged(Vector2.zero);
                }
            }
        }

        void Awake()
        {
            _joystick = GetComponent<Joystick>();
        }

        void Update()
        {
            IsPressed = _joystick.Direction != Vector2.zero;

            if (IsPressed)
            {
                Trigger_ValueChanged(_joystick.Direction);
            }
        }
        
        void Trigger_ValueChanged(Vector2 value)
        {
            foreach (var actionEvent in inputActionEvents)
            {
                actionEvent.Raise(actionName, value);
                if (debug)
                    Debug.Log($"[{value}] {actionEvent.name} from {gameObject.name}", this);
            }
        }
    }
}
