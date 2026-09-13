using System;
using System.Collections.Generic;
using RAXY.InputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Sample consumer that subscribes to one or more InputActionEventSOs and logs each raise.
/// </summary>
public class SampleInputDebugListener : MonoBehaviour
{
    [SerializeField]
    List<InputActionEventSO> events = new();

    [TitleGroup("Last Event")]
    [ShowInInspector, ReadOnly]
    string lastActionName;

    [TitleGroup("Last Event")]
    [ShowInInspector, ReadOnly]
    bool lastBoolValue;

    [TitleGroup("Last Event")]
    [ShowInInspector, ReadOnly]
    Vector2 lastVector2Value;

    readonly Dictionary<InputActionEventSO, Action<InputContext>> _handlers = new();

    void OnEnable()
    {
        foreach (var so in events)
        {
            if (so == null || _handlers.ContainsKey(so))
                continue;

            Action<InputContext> handler = OnInput;
            _handlers[so] = handler;
            so.Subscribe(handler);
        }
    }

    void OnDisable()
    {
        foreach (var pair in _handlers)
        {
            if (pair.Key != null)
                pair.Key.Unsubscribe(pair.Value);
        }

        _handlers.Clear();
    }

    void OnInput(InputContext ctx)
    {
        lastActionName = ctx.ActionName;
        lastBoolValue = ctx.BoolValue;
        lastVector2Value = ctx.Vector2Value;

        Debug.Log(
            $"[SampleInputDebugListener] Action={ctx.ActionName} Bool={ctx.BoolValue} Vector2={ctx.Vector2Value}",
            this);
    }
}
