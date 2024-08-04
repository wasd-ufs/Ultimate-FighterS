using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : CharacterState
{
    [Header("Transitions")] 
    [SerializeField] private CharacterState grounded;
    [SerializeField] private CharacterState wall;
    
    [Header("Vertical")]
    [SerializeField] private float gravityForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fastFallGravity;
    
    private bool isFastFalling;
    
    [Header("Horizontal")]
    [SerializeField] private float acceleration;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    
    public override void Enter()
    {
        isFastFalling = false;
    }

    public override void Process()
    {
        isFastFalling = isFastFalling || !input.IsSpecialBeingHeld() || body.GetSpeedOnAxis(body.Down) > 0;
    }

    public override void PhysicsProcess()
    {
        var gravity = isFastFalling ? fastFallGravity : gravityForce;
        body.Accelerate(body.Down * gravity);
        
        body.LimitSpeed(body.Down, maxFallSpeed);
        body.MoveSmoothly(body.Right, input.GetDirection().x, acceleration, turnAcceleration, deceleration, maxSpeed);

        if (body.IsOnFloor())
        {
            machine.TransitionTo(grounded);
            return;
        }
        
        if (ShouldWallSlide())
        {
            machine.TransitionTo(wall);
        }
    }
    private bool ShouldWallSlide() =>
        (Vector2.Dot(body.LeftWallNormal, input.GetDirection()) < -0.1f) ||
        (Vector2.Dot(body.RightWallNormal, input.GetDirection()) < -0.1f);

}
