using UnityEngine;

public abstract class PlayerState : State
{
    public StateMachine stateMachine { get; set; }
    public PlayerInputSystem input { get; set; }
    public CharacterBody2D body { get; set; }

    public virtual void Process() {}

    public virtual void PhysicsProcess() {}

}
