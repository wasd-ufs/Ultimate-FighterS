using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachine machine;
    public State(StateMachine machine)
    {
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }
}
