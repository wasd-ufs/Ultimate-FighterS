using UnityEngine;

public class MomentumFlipBehaviour : CharacterState
{
    private Vector2 SurfaceForward => body.IsOnFloor() ? body.GetFloorRight()
        : body.IsOnCeiling() ? body.GetCeilingLeft()
        : body.IsOnLeftWall() ? body.GetLeftWallUp()
        : body.IsOnRightWall() ? body.GetRightWallDown()
        : Vector2.zero;
    
    public override void Enter()
    {
        var directionOrtho = Vector2.Dot(SurfaceForward, input.GetDirection());
        var velocityOrtho = Vector2.Dot(SurfaceForward, body.Velocity);
        
        if (directionOrtho * velocityOrtho < 0f)
            body.ModifyVelocityOnAxis(SurfaceForward, vel => -vel);
    }
}