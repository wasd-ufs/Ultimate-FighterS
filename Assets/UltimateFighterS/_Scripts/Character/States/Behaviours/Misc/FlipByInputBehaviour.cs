using UnityEngine;

/// <summary>
/// Vira o personagem baseado na direção do player e velocidade do corpo 
/// </summary>
public class FlipByInputBehaviour : CharacterState
{
    public override void StateUpdate()
    {
        if ((IsFacingRight() && Body.GetSpeedRight() < 0) || (!IsFacingRight() && Body.GetSpeedLeft() < 0))
            Flip();
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

    /// <summary>
    /// Indica se o corpo atualmente olha para a direita, baseado na escala do pivô
    /// </summary>
    /// <returns>Booleano que indica se o corpo está olhando para a direita</returns>
    private bool IsFacingRight()
    {
        return FlipPivotPoint.localScale.x > 0;
    }
}