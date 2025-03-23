using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Possíveis direções fixas que o personagem pode ser forçado a olhar
/// </summary>
public enum ForcedFacingDirection
{
    Left,
    Right,
    WallNormal,
    FlipAnyway
}

/// <summary>
/// Força o corpo a olhar para uma direção específica ao entrar no estado
/// </summary>
public class ForceFacingDirectionBehaviour : CharacterState
{
    [FormerlySerializedAs("_setFacingDirection")] 
    [FormerlySerializedAs("facingDirection")] 
    [SerializeField] private ForcedFacingDirection _forcedFacingDirection;

    public override void Enter()
    {
        if (IsFacingAway())
            Flip();
    }

    /// <summary>
    /// Indica se o corpo atualmente olha para uma direção contrária à que deve ser forçado a olhar, observando a escala do pivô
    /// </summary>
    /// <returns>Booleano que indica se o corpo olha para uma direção contrária à que deve ser forçado a olhar</returns>
    private bool IsFacingAway()
    {
        return _forcedFacingDirection switch
        {
            ForcedFacingDirection.Left => FlipPivotPoint.localScale.x > 0,
            ForcedFacingDirection.Right => FlipPivotPoint.localScale.x < 0,
            ForcedFacingDirection.WallNormal => GetWallNormal().x * FlipPivotPoint.localScale.x < 0,
            ForcedFacingDirection.FlipAnyway => true
        };
    }

    /// <summary>
    /// Indica qual das duas normais da parede deve ser utilizada.
    /// No caso do player estar entre duas paredes, a da esquerda possui preferência.
    /// Ainda pode retornar Vector2.zero se o jogador não estiver em contato com nenhuma parede
    /// </summary>
    /// <returns>O vetor normal da parede</returns>
    private Vector2 GetWallNormal()
    {
        return Body.IsOnLeftWall()
            ? Body.LeftWallNormal
            : Body.RightWallNormal;
    }

    /// <summary>
    /// Vira o personagem em torno do pivô para a direção contraria à que está olhando atualmente 
    /// </summary>
    /// TODO: Passar essa função para o CharacterBody
    private void Flip()
    {
        Vector2 scale = FlipPivotPoint.localScale;
        scale.x *= -1;
        FlipPivotPoint.localScale = scale;
    }
}