using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using System;
using RAXY.Utility;

namespace RAXY.InputSystem
{
    public class CursorManager : Singleton<CursorManager>
    {
        public InputActionEventSO showCursorEvent;

        void Start()
        {
            showCursorEvent.Subscribe(ShowCursorChangedHandler);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            showCursorEvent.Unsubscribe(ShowCursorChangedHandler);
        }

        void ShowCursorChangedHandler(InputContext ctx)
        {
            if (ctx.BoolValue)
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
            }
        }

        [TitleGroup("Cursor")]
        public bool forceShow = false;
        [TitleGroup("Cursor")]
        public bool isShowCursor = false;
        [TitleGroup("Cursor")]
        public bool isDetectMouseInput;
        [TitleGroup("Cursor")]
        public bool isDetectKeyboardInput;

        [HorizontalGroup("Cursor/Buttons")]
        [Button]
        public void ShowCursor()
        {
            isShowCursor = true;
            isDetectMouseInput = false;

            Cursor.lockState = CursorLockMode.None;

            isDetectKeyboardInput = false;
        }

        [HorizontalGroup("Cursor/Buttons")]
        [Button]
        public void HideCursor()
        {
            if (forceShow)
                return;

            isShowCursor = false;

#if UNITY_EDITOR
            isDetectMouseInput = true;
            Cursor.lockState = CursorLockMode.Locked;
#else
            Cursor.lockState = CursorLockMode.None;
            detectMouseInput = false;
#endif

            isDetectKeyboardInput = true;
        }

        /// <summary>
        /// Can only be unlocked using ForceHideCursor()
        /// </summary>
        /// <returns></returns>
        [HorizontalGroup("Cursor/Forces")]
        [Button]
        public void ForceShowCursor()
        {
            forceShow = true;
            ShowCursor();
        }

        [HorizontalGroup("Cursor/Forces")]
        [Button]
        public void ForceHideCursor()
        {
            forceShow = false;
            HideCursor();
        }
    }
}