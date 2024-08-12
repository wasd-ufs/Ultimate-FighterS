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
    [SerializeField] private ActionSystem upAerial;
    [SerializeField] private ActionSystem downAerial;
    [SerializeField] private ActionSystem forwardAerial;
    [SerializeField] private ActionSystem backAerial;
    [SerializeField] private ActionExecutor executor;

    [Header("Special Movements")] 
    [SerializeField] private CharacterState upSpecial;
    [SerializeField] private CharacterState downSpecial;
    [SerializeField] private CharacterState forwardSpecial;
    [SerializeField] private CharacterState backSpecial;

    private readonly List<Vector2> directions =
        new List<Vector2> { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    
    public override void Enter()
    {
        isFastFalling = false;
    }

    public override void Process()
    {
        isFastFalling = isFastFalling || !input.IsSpecialBeingHeld() || body.GetSpeedOnAxis(body.Down) > 0;
        TryDoAttack();
        TryDoSpecialMovement();
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

    private void TryDoAttack()
    {
        if (!input.IsAttackJustPressed())
            return;

        var direction = input.GetDirection();
        if (direction == Vector2.zero)
            return;
        
        var closest = VectorUtils.Closest(direction, directions);
        
        if (closest == Vector2.left)
            executor.TryRun(backAerial);
        
        else if (closest == Vector2.right)
            executor.TryRun(forwardAerial);
            
        else if (closest == Vector2.up)
            executor.TryRun(upAerial);
        
        else if (closest == Vector2.down)
            executor.TryRun(downAerial);
    }

    private void TryDoSpecialMovement()
    {
        if (!input.IsSpecialJustPressed())
            return;

        executor.TryFinish();
        if (executor.IsRunning())
            return;
        
        var direction = input.GetDirection();
        if (direction == Vector2.zero)
            return;
        
        var closest = VectorUtils.Closest(direction, directions);
        
        if (closest == Vector2.left)
            machine.TransitionTo(backSpecial);
        
        else if (closest == Vector2.right)
            machine.TransitionTo(forwardSpecial);
            
        else if (closest == Vector2.up)
            machine.TransitionTo(upSpecial);
        
        else if (closest == Vector2.down)
            machine.TransitionTo(downSpecial);
    }
    
    private bool ShouldWallSlide() =>
        (Vector2.Dot(body.LeftWallNormal, input.GetDirection()) < -0.1f) ||
        (Vector2.Dot(body.RightWallNormal, input.GetDirection()) < -0.1f);
}
