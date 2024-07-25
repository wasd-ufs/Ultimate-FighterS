using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<TS> : MonoBehaviour where TS: State
{
    [SerializeField] private TS initialState;
    private TS current;

    public TS Current => current;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        current = initialState;
    }

    public void TransitionTo(TS next)
    {
        if (next is null)
            return;

        current?.Exit();
        
        current = next;
        Inject(current);
        
        current.Enter();
    }
    
    protected virtual void Inject(TS state) {}
}

public abstract class State : MonoBehaviour
{
    public virtual void Enter() {}
    public virtual void Exit() {}
}
