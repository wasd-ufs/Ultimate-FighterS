using UnityEngine;

///<summary>
/// Classe base para os estados do personagem.
/// Define propriedades e métodos comuns para a transição e processamento dos estados.
///</summary>
public abstract class CharacterState : State
{
    public StateMachine<CharacterState> Machine { get; set; }
    public InputSystem Input { get; set; }
    public CharacterBody2D Body { get; set; }
    public InputBuffer InputBuffer { get; set; }
    public Transform FlipPivotPoint { get; set; }
    public ParticleSystem DustParticles { get; set; }

    public virtual void Process() {}

    public virtual void PhysicsProcess() {}

    public virtual void OnCeilingEnter(Vector2 normal) {}
    public virtual void OnCeilingExit(Vector2 normal) {}

    public virtual void OnFloorEnter(Vector2 normal) {}
    public virtual void OnFloorExit(Vector2 normal) {}

    public virtual void OnLeftWallEnter(Vector2 normal) {}
    public virtual void OnLeftWallExit(Vector2 normal) {}

    public virtual void OnRightWallEnter(Vector2 normal) {}
    public virtual void OnRightWallExit(Vector2 normal) {}
    
    public enum CharacterBasis
    {
        Floor,
        Wall,
        Ceiling,
        Body,
        BodyForward,
        XY,
        Input,
        Velocity
    }

    ///<summary>
    /// Retorna os vetores base correspondentes ao referencial escolhido.
    ///</summary>
    ///<param name="basis">O referencial desejado.</param>
    ///<returns>Um par de vetores que representam a direção primária e sua ortogonal associada.</returns>
    ///<author>Davi Fontes</author>
    public (Vector2, Vector2) GetBasis(CharacterBasis basis) => basis switch
    {
        CharacterBasis.Floor => (Body.GetFloorRight(), Body.FloorNormal),
        CharacterBasis.Wall => Body.IsOnLeftWall() ? (Body.LeftWallNormal, Body.GetLeftWallUp()) : (Body.RightWallNormal, Body.GetRightWallUp()),
        CharacterBasis.Ceiling => (Body.GetCeilingLeft(), Body.CeilingNormal),
        CharacterBasis.Body => (Body.Right, Body.Up),
        CharacterBasis.BodyForward => (Body.Right * Mathf.Sign(FlipPivotPoint.lossyScale.x), Body.Up),
        CharacterBasis.XY => (Vector2.right, Vector2.up),
        CharacterBasis.Input => (Input.GetDirection(), VectorUtils.Orthogonal(Input.GetDirection())),
        CharacterBasis.Velocity => (Body.Velocity.normalized, VectorUtils.Orthogonal(Body.Velocity.normalized))
    };
}
