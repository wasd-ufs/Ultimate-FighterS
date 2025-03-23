using UnityEngine;

/// <summary>
/// Seguindo a lógica de Momentum Flip, caso o jogador direcione contráriamente ao vetor frente da superfície ao entrar nesse estado, sua velocidade é espelhada no eixo do vetor frente da superfície
/// </summary>
public class MomentumFlipBehaviour : CharacterState
{
    private Vector2 SurfaceForward => Body.IsOnFloor() ? Body.GetFloorRight()
        : Body.IsOnCeiling() ? Body.GetCeilingLeft()
        : Body.IsOnLeftWall() ? Body.GetLeftWallUp()
        : Body.IsOnRightWall() ? Body.GetRightWallDown()
        : Vector2.zero;

    public override void Enter()
    {
        float directionOrtho = Vector2.Dot(SurfaceForward, Input.GetDirection());
        float velocityOrtho = Vector2.Dot(SurfaceForward, Body.Velocity);

        if (directionOrtho * velocityOrtho < 0f)
            Body.ModifyVelocityOnAxis(SurfaceForward, vel => -vel);
    }
}