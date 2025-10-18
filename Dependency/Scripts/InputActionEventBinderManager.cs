using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RAXY.InputSystem
{
    public class InputActionEventBinderManager : MonoBehaviour
    {
        public static InputActionEventBinderManager Instance { get; private set; }

        [TitleGroup("Input Action Setup")]
        [TableList]
        public List<InputActionEventBinding> InputActionBindings;

#if UNITY_EDITOR
        [TitleGroup("All Event SO")]
        [ShowInInspector, ReadOnly]
        private List<InputActionEventSO> _allInputActionEvents = new();

        [HorizontalGroup("All Event SO/Op")]
        [Button("Find All")]
        private void FindAllInputActionEventSO()
        {
            _allInputActionEvents.Clear();

            string[] guids = AssetDatabase.FindAssets("t:InputActionEventSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InputActionEventSO so = AssetDatabase.LoadAssetAtPath<InputActionEventSO>(path);
                if (so != null)
                    _allInputActionEvents.Add(so);
            }

            Debug.Log($"[InputActionManager] Found {_allInputActionEvents.Count} InputActionEventSO assets.");
        }

        [HorizontalGroup("All Event SO/Op")]
        [Button("Clean Up")]
        private void CleanUpAllInputActionEventSO()
        {
            FindAllInputActionEventSO();

            foreach (var so in _allInputActionEvents)
            {
                so.ClearAllListeners();
            }

            Debug.Log($"[InputActionManager] Cleared listeners from {_allInputActionEvents.Count} InputActionEventSO assets.");
        }
#endif

        private void Awake()
        {
            Instance = this;

            foreach (var inputEntry in InputActionBindings)
            {
                inputEntry.Init();
            }
        }

        private void OnDestroy()
        {
            foreach (var inputEntry in InputActionBindings)
            {
                inputEntry.Dispose();
            }

#if UNITY_EDITOR
            // Auto cleanup when exiting Play Mode (optional but handy)
            CleanUpAllInputActionEventSO();
#endif
        }
    }

    [Serializable]
    public class InputActionEventBinding
    {
        [FormerlySerializedAs("inputAction")]
        public InputActionReference unityInputAction;
        public InputActionEventSO actionEventSO;

        public void Init()
        {
            if (unityInputAction?.action == null || actionEventSO == null)
                return;

            unityInputAction.action.performed += OnPerformed;
            unityInputAction.action.canceled += OnCanceled;
            unityInputAction.action.Enable();
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

        public void Dispose()
        {
            if (unityInputAction?.action != null)
            {
                unityInputAction.action.performed -= OnPerformed;
                unityInputAction.action.canceled -= OnCanceled;
            }
        }
    }
}
