using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Enumera modos de preservação de momentum para a aplicação do impulso
/// </summary>
public enum MomentumPreservation
{
    None,
    KeepImpulseTangent,
    KeepImpulseDirection,
    Keep
}

/// <summary>
/// Aplica um impulso, seguindo um modo de preservação e um vetor impulso e sua base
/// </summary>
public class ImpulseBehaviour : CharacterState
{
    [FormerlySerializedAs("impulse")]
    [Header("Impulse")] 
    [SerializeField] private Vector2 _impulse;
    // TODO: Encapsular a ideia de base numa classe ou record próprio
    [FormerlySerializedAs("impulseBasis")] [SerializeField] private CharacterBasis _impulseBasis;

    [FormerlySerializedAs("momentumPreservation")] [SerializeField] private MomentumPreservation _momentumPreservation;

    public override void Enter()
    {
        Vector2 impulse = GetFinalImpulse();
        ApplyMomentumPreservation(impulse);
        Body.ApplyImpulse(impulse);

        Body.FixedUpdate();
    }

    /// <summary>
    /// Calcula o impulso final considerando a base do vetor impulso
    /// </summary>
    /// <returns>O vetor impulso na base XY</returns>
    /// TODO: Passar esse método para a futura classe das Direções com base
    private Vector2 GetFinalImpulse()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_impulseBasis);
        return _impulse.x * forward + _impulse.y * up;
    }

    /// <summary>
    /// Aplica a preservação de momentum especificada em _momentumPreservation
    /// </summary>
    /// <param name="impulse">O impulso a ser aplicado</param>
    private void ApplyMomentumPreservation(Vector2 impulse)
    {
        switch (_momentumPreservation)
        {
            case MomentumPreservation.None:
                Body.SetVelocity(Vector2.zero);
                break;

            case MomentumPreservation.KeepImpulseTangent:
                Body.SetSpeed(impulse.normalized, 0f);
                break;

            case MomentumPreservation.KeepImpulseDirection:
                Body.SetSpeed(VectorUtils.Orthogonal(impulse).normalized, 0f);
                break;
        }
    }
}