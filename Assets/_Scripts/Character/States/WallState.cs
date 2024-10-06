using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallState : CharacterState
{
    [Header("Transitions")]
    [SerializeField] private CharacterState grounded;
    [SerializeField] private CharacterState airborne;

    [Header("Vertical")]
    [SerializeField] private float gravityForce;
    [SerializeField] private float maxFallSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpAngle;
    [SerializeField] private float jumpForce;

    private bool isNormalLeft;
    private Vector2 down => isNormalLeft ? body.GetLeftWallDown() : body.GetRightWallDown();
    private Vector2 normal => isNormalLeft ? body.LeftWallNormal : body.RightWallNormal;
    public override void Enter()
    {
        isNormalLeft = body.IsOnLeftWall();
        
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

        lag.OnLag(machine);

        if (input.IsSpecialJustPressed())
        {
            WallJump();
        }

        if (Vector2.Dot(normal, input.GetDirection()) > 0.1f)
        {
            machine.TransitionTo(airborne);
        }

        facingDirection.DirectionFace(input.GetDirection());
    }
    public override void PhysicsProcess()
    {
        body.ConvergeSpeed(down, gravityForce, maxFallSpeed);
        if (body.IsOnFloor())
        {
            machine.TransitionTo(grounded);
            return;
        }
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        if (isNormalLeft) machine.TransitionTo(airborne);
    }

    public override void OnRightWallExit(Vector2 normal) 
    {
        if (!isNormalLeft) machine.TransitionTo(airborne);
    }

    public void WallJump()
    {
        body.SetSpeed(normal, jumpForce * Mathf.Cos(Mathf.Deg2Rad * jumpAngle));
        body.SetSpeed(-down, jumpForce * Mathf.Sin(Mathf.Deg2Rad * jumpAngle));
    }
}
