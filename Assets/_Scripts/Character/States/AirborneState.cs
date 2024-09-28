using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxOverspeed;

    [Header("Actions")] 
    [SerializeField] private DirectionalTrigger attacks;
    [SerializeField] private DirectionalTrigger specials;
    [SerializeField] private ActionExecutor executor;
    
    public override void Enter()
    {
        isFastFalling = false;

        float direction = player.localScale.x;

        if (direction == 1)
        {
            facingDirection.facingRight = true;
        }
        if (direction == -1)
        {
            facingDirection.facingRight = false;
        }
    }

    public override void Process()
    {
        isFastFalling = isFastFalling || !input.IsSpecialBeingHeld() || body.GetSpeedOnAxis(body.Down) > 0;

        if (input.IsAttackJustPressed()) 
        { 
            attacks.Trigger(input.GetDirection());
            facingDirection.DirectionFace(input.GetDirection());
        } 
        else if (!executor.IsRunning() && input.IsSpecialJustPressed())
        { 
            specials.Trigger(input.GetDirection());
            facingDirection.DirectionFace(input.GetDirection());
        }
    }

    public override void PhysicsProcess()
    {
        body.MoveSmoothly(body.Right, input.GetDirection().x, acceleration, turnAcceleration, deceleration, maxMoveSpeed, deceleration, maxOverspeed);
        
        var gravity = isFastFalling ? fastFallGravity : gravityForce;
        body.Accelerate(body.Down * gravity);
        body.LimitSpeed(body.Down, maxFallSpeed);

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
