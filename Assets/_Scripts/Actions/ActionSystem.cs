using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionSystem : State
{
    public StateMachine<ActionSystem> machine { get; set; }

    public virtual void OnFinishRequested(){}
    public void Finish()
    {
        machine?.TransitionTo(machine.initialState);
    }
}
