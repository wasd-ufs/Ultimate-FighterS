using UnityEngine;

/// <summary>
/// Bases de sistemas de coordenadas baseadas nas informações de corpo e input do jogador.
/// </summary>
/// TODO: Passar essa parte para uma classe dedicada de Bases
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

/// <summary>
/// Classe de todas os Estados e Comportamentos do personagem
/// </summary>
public abstract class CharacterState : State
{
    public StateMachine<CharacterState> Machine { get; set; }
    public InputSystem Input { get; set; }
    public CharacterBody2D Body { get; set; }
    public InputBuffer InputBuffer { get; set; }
    public Transform FlipPivotPoint { get; set; }
    public ParticleSystem DustParticles { get; set; }

    /// <summary>
    /// Evento que ocorre a cada Frame.
    /// Equivalente ao Update da Unity
    /// </summary>
    public virtual void StateUpdate()
    {
    }

    /// <summary>
    /// Evento que ocorre a cada Frame Fixo.
    /// Equivalente ao FixedUpdate da Unity
    /// </summary>
    public virtual void StateFixedUpdate()
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo atinge um teto
    /// </summary>
    /// <param name="normal">A normal do teto</param>
    public virtual void OnCeilingEnter(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo sai do teto
    /// </summary>
    /// <param name="normal">A normal do teto</param>
    public virtual void OnCeilingExit(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo atinge um chão
    /// </summary>
    /// <param name="normal">A normal do chão</param>
    public virtual void OnFloorEnter(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo sai do chão
    /// </summary>
    /// <param name="normal">A normal do chão</param>
    public virtual void OnFloorExit(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo atinge uma parede esquerda
    /// </summary>
    /// <param name="normal">A normal da parede</param>
    public virtual void OnLeftWallEnter(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo sai da parede esquerda
    /// </summary>
    /// <param name="normal">A normal da parede</param>
    public virtual void OnLeftWallExit(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo atinge uma parede direita
    /// </summary>
    /// <param name="normal">A normal da parede</param>
    public virtual void OnRightWallEnter(Vector2 normal)
    {
    }

    /// <summary>
    /// Evento que ocorre quando o corpo sai da parede esquerda
    /// </summary>
    /// <param name="normal">A normal da parede</param>
    public virtual void OnRightWallExit(Vector2 normal)
    {
    }

    /// <summary>
    /// Retorna os vetores direita e cima relacionados a uma base
    /// </summary>
    /// <param name="basis">a base que se quer quebrar</param>
    /// <returns>A tupla (direita, cima) da base</returns>
    /// TODO: Passar essa função para uma classe mais apropriada
    public (Vector2, Vector2) GetBasis(CharacterBasis basis)
    {
        return basis switch
        {
            CharacterBasis.Floor => (Body.GetFloorRight(), Body.FloorNormal),
            CharacterBasis.Wall => Body.IsOnLeftWall()
                ? (Body.LeftWallNormal, Body.GetLeftWallUp())
                : (Body.RightWallNormal, Body.GetRightWallUp()),
            CharacterBasis.Ceiling => (Body.GetCeilingLeft(), Body.CeilingNormal),
            CharacterBasis.Body => (Body.Right, Body.Up),
            CharacterBasis.BodyForward => (Body.Right * Mathf.Sign(FlipPivotPoint.lossyScale.x), Body.Up),
            CharacterBasis.XY => (Vector2.right, Vector2.up),
            CharacterBasis.Input => (Input.GetDirection(), VectorUtils.Orthogonal(Input.GetDirection())),
            CharacterBasis.Velocity => (Body.Velocity.normalized, VectorUtils.Orthogonal(Body.Velocity.normalized))
        };
    }
}