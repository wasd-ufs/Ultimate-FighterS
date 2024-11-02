using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public abstract class StateMachine<TS> : MonoBehaviour where TS: State
{
    [SerializeField] public TS initialState;
    protected TS current;

    public TS Current => current;

    private void Start()
    {
        Reset();
        OnStart();
    }

    public void Reset()
    {
        TransitionTo(initialState);
    }

    public void TransitionTo(TS next)
    {
        current?.Exit();

        current = next;
        OnTransition();
        
        current.Enter();
    }
    
    protected virtual void OnStart() {}
    protected virtual void OnTransition() {}
}

public abstract class State : MonoBehaviour
{
    public virtual void Enter() {}
    public virtual void Exit() {}
}
