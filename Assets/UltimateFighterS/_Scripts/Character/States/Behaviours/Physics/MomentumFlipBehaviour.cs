using UnityEngine;

///<summary>
/// Controla a invers�o da dire��o da velocidade do personagem ao encontrar uma superf�cie.
///</summary>
public class MomentumFlipBehaviour : CharacterState
{
    ///<summary>
    /// Obt�m a dire��o ortogonal da superf�cie em que o personagem est� colidindo.
    ///</summary>
    ///<author>Davi Fontes</author>
    private Vector2 SurfaceForward => Body.IsOnFloor() ? Body.GetFloorRight()
        : Body.IsOnCeiling() ? Body.GetCeilingLeft()
        : Body.IsOnLeftWall() ? Body.GetLeftWallUp()
        : Body.IsOnRightWall() ? Body.GetRightWallDown()
        : Vector2.zero;

    ///<summary>
    /// Inverte a velocidade do personagem se ele estiver se movendo contra a superf�cie de contato.
    ///</summary>
    ///<author>Davi Fontes</author>
    public override void Enter()
    {
        float _directionOrtho = Vector2.Dot(SurfaceForward, Input.GetDirection());
        float _velocityOrtho = Vector2.Dot(SurfaceForward, Body.Velocity);
        
        if (_directionOrtho * _velocityOrtho < 0f)
            Body.ModifyVelocityOnAxis(SurfaceForward, vel => -vel);
    }
}