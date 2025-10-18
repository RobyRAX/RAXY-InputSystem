using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using RAXY.Event;

namespace RAXY.InputSystem
{
    [CreateAssetMenu(menuName = "RAXY/Input System/Input Action Event")]
    public class InputActionEventSO : EventBaseSO<InputContext>
    {
        public void Raise(string actionName, bool value) => Raise(new InputContext(actionName, value));
        public void Raise(string actionName, Vector2 value) => Raise(new InputContext(actionName, value));
        public void Raise(bool value) => Raise(new InputContext(value));
        public void Raise(Vector2 value) => Raise(new InputContext(value));
    }

    [Serializable]
    public struct InputContext
    {
        public string ActionName;
        public bool BoolValue;
        public Vector2 Vector2Value;

        public InputContext(string actionName, bool boolValue)
        {
            ActionName = actionName;
            BoolValue = boolValue;
            Vector2Value = Vector2.zero;
        }

        public InputContext(string actionName, Vector2 vectorValue)
        {
            ActionName = actionName;
            BoolValue = false;
            Vector2Value = vectorValue;
        }

        public InputContext(bool boolValue)
        {
            ActionName = "";
            BoolValue = boolValue;
            Vector2Value = Vector2.zero;
        }

        public InputContext(Vector2 vectorValue)
        {
            ActionName = "";
            BoolValue = false;
            Vector2Value = vectorValue;
        }

        public InputContext(bool boolValue, Vector2 vector2Value)
        {
            ActionName = "";
            BoolValue = boolValue;
            Vector2Value = vector2Value;
        }
    }
}