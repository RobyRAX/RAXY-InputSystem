using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RAXY.InputSystem
{
    public abstract class InputActionRaiserBase : MonoBehaviour
    {
        [SerializeField]
        protected string actionName;
        
        [SerializeField]
        protected bool debug;

        [Tooltip("List of InputActionEventSO that will be triggered when this button is pressed or released.")]
        public List<InputActionEventSO> inputActionEvents = new();
    }
}
