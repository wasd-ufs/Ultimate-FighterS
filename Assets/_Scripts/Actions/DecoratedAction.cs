using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DecoratedAction : ActionSystem
{
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;
    public ActionSystem action;


    public override void Enter()
    {
        action.machine = machine;
        OnEnter?.Invoke();
        action.Enter();
    }

    public override void Exit()
    {
        action.machine = machine;
        OnExit?.Invoke();
        action.Exit();
    }
}
