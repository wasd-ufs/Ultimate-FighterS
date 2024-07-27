using UnityEngine;

public abstract class CharacterState : State
{
    public StateMachine<CharacterState> machine { get; set; }
    public InputSystem input { get; set; }
    public CharacterBody2D body { get; set; }

    public virtual void Process() {}

    public virtual void PhysicsProcess() {}

    public virtual void OnCeilingEnter() {}
    public virtual void OnCeilingExit() {}

    public virtual void OnFloorEnter() {}
    public virtual void OnFloorExit() {}

    public virtual void OnLeftWallEnter() {}
    public virtual void OnLeftWallExit() {}

    public virtual void OnRightWallEnter() {}
    public virtual void OnRightWallExit() {}

    public virtual void OnWallEnter() {}
    public virtual void OnWallExit() {}
}
