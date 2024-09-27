using UnityEngine;

public abstract class CharacterState : State
{
    public StateMachine<CharacterState> machine { get; set; }
    public InputSystem input { get; set; }
    public CharacterBody2D body { get; set; }
    public FacingDirection facingDirection { get; set; }
    public Transform player;

    public virtual void Process() {}

    public virtual void PhysicsProcess() {}

    public virtual void OnCeilingEnter(Vector2 normal) {}
    public virtual void OnCeilingExit(Vector2 normal) {}

    public virtual void OnFloorEnter(Vector2 normal) {}
    public virtual void OnFloorExit(Vector2 normal) {}

    public virtual void OnLeftWallEnter(Vector2 normal) {}
    public virtual void OnLeftWallExit(Vector2 normal) {}

    public virtual void OnRightWallEnter(Vector2 normal) {}
    public virtual void OnRightWallExit(Vector2 normal) {}

    public virtual void OnWallEnter(Vector2 normal) {}
    public virtual void OnWallExit(Vector2 normal) {}
}
