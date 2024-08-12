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
    [SerializeField] private ActionSystem upTilt;
    [SerializeField] private ActionSystem downTilt;
    [SerializeField] private ActionSystem forwardTilt;
    [SerializeField] private ActionSystem backTilt;
    [SerializeField] private ActionExecutor executor;

    private readonly List<Vector2> directions =
        new List<Vector2> { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    
    public override void Process()
    {
        if (input.IsSpecialJustPressed())
        {
            body.SetSpeed(body.Up, jumpForce);
            machine.TransitionTo(airborne);
            return;
        }
        
        TryDoAttack();
    }

    public override void PhysicsProcess()
    {
        body.MoveSmoothly(body.GetFloorRight(), input.GetDirection().x, acceleration, turnAcceleration, deceleration, maxSpeed, overspeedDeceleration);
        if (!body.IsOnFloor()) machine.TransitionTo(airborne);
    }
    
    private void TryDoAttack()
    {
        if (!input.IsAttackJustPressed())
            return;

        var direction = input.GetDirection();
        if (direction == Vector2.zero)
            return;
        
        var closest = VectorUtils.Closest(direction, directions);
        
        if (closest == Vector2.left)
            executor.TryRun(backTilt);
        
        else if (closest == Vector2.right)
            executor.TryRun(forwardTilt);
            
        else if (closest == Vector2.up)
            executor.TryRun(upTilt);
        
        else if (closest == Vector2.down)
            executor.TryRun(downTilt);
    }
}

