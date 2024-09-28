using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : CharacterState
{
    [Header("Transitions")]
    [SerializeField] private CharacterState airborne;

    [Header("Horizontal")] 
    [SerializeField] private float acceleration;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float overspeedDeceleration;
    
    [Header("Vertical")]
    [SerializeField] private float jumpForce;

    [Header("Actions")] 
    [SerializeField] private DirectionalTrigger attacks;
    [SerializeField] private ActionExecutor executor;

    public override void Enter()
    {
        if (executor.IsRunning())
            executor.Current.Finish();

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
        if (input.IsSpecialJustPressed())
        {
            body.SetSpeed(body.Up, jumpForce);
            machine.TransitionTo(airborne);
            return;
        }
        
        if (input.IsAttackJustPressed())
        {
            attacks.Trigger(input.GetDirection());
            facingDirection.DirectionFace(input.GetDirection());
        }
    }

    public override void PhysicsProcess()
    {
        body.MoveSmoothly(body.GetFloorRight(), input.GetDirection().x, acceleration, turnAcceleration, deceleration, maxSpeed, overspeedDeceleration);
        if (!body.IsOnFloor()) machine.TransitionTo(airborne);
    }
}

