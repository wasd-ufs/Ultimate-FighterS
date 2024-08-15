using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CancelableAction : ActionSystem
{
    [SerializeField] private UnityAction OnCancel;
    public bool isCancelable;
    public ActionSystem action;

    public override void OnFinishRequested()
    {
        if (isCancelable)
        {
            action.Finish();
            OnCancel?.Invoke();
        }
    }

    public override void Enter()
    {
        action.machine = machine;
        action.Enter();
    }

    public override void Exit()
    {
        action.machine = machine;
        action.Exit();
    }
}
