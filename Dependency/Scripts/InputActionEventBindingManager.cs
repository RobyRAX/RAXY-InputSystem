using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using RAXY.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RAXY.InputSystem
{
    public class InputActionEventBindingManager : Singleton<InputActionEventBindingManager>
    {
        [TitleGroup("Bindings")]
        [TableList]
        public List<InputActionEventBinding> InputActionBindings;

        protected override void Awake()
        {
            base.Awake();
            Bind();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Unbind();
        }

        [HorizontalGroup]
        [Button]
        public void Bind()
        {
            foreach (var inputEntry in InputActionBindings)
            {
                inputEntry.Bind();
            }
        }
        
        [HorizontalGroup]
        [Button]
        public void Unbind()
        {
            foreach (var inputEntry in InputActionBindings)
            {
                inputEntry.Unbind();
            }
        }
    }

    [Serializable]
    public class InputActionEventBinding
    {
        [FormerlySerializedAs("inputAction")]
        public InputActionReference unityInputAction;
        public InputActionEventSO actionEventSO;

        public void Bind()
        {
            if (unityInputAction?.action == null || actionEventSO == null)
                return;

            unityInputAction.action.performed += OnPerformed;
            unityInputAction.action.canceled += OnCanceled;
            unityInputAction.action.Enable();
        }

        public void Unbind()
        {
            if (unityInputAction?.action != null)
            {
                unityInputAction.action.performed -= OnPerformed;
                unityInputAction.action.canceled -= OnCanceled;
            }
        }

        private void OnPerformed(InputAction.CallbackContext ctx)
        {
            InputAction action = unityInputAction.action;
            string actionName = action.name;

            if (action.type == InputActionType.Button)
            {
                bool pressed = ctx.phase == InputActionPhase.Performed;
                actionEventSO.Raise(actionName, pressed);
            }
            else // Value / PassThrough
            {
                Vector2 value = Vector2.zero;

                if (ctx.ReadValueAsObject() is Vector2 v)
                    value = v;
                else if (ctx.ReadValueAsObject() is float f)
                    value = new Vector2(f, 0);

                actionEventSO.Raise(actionName, value);
            }
        }

        private void OnCanceled(InputAction.CallbackContext ctx)
        {
            InputAction action = unityInputAction.action;
            string actionName = action.name;

            if (action.type == InputActionType.Button)
            {
                bool pressed = ctx.phase == InputActionPhase.Performed;
                actionEventSO.Raise(actionName, pressed);
            }
            else // Value / PassThrough
            {
                Vector2 value = Vector2.zero;

                if (ctx.ReadValueAsObject() is Vector2 v)
                    value = v;
                else if (ctx.ReadValueAsObject() is float f)
                    value = new Vector2(f, 0);

                actionEventSO.Raise(actionName, value);
            }
        }
    }
}
