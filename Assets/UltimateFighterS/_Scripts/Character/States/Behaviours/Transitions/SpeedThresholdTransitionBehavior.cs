using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// As possíveis comparações de velocidade
/// </summary>
public enum SpeedComparisonType
{
    Higher,
    Lower,
    ApproximatelyEqual,
    ApproximatelyDifferent
}

/// <summary>
/// Realiza a transição assim que o corpo ultrapassa um limite de velocidade, conforme o tipo de comparação especificada e o eixo da checagem
/// </summary>
public class SpeedThresholdTransitionBehavior : CharacterState
{
    [FormerlySerializedAs("axis")]
    [Header("Axis")] 
    [SerializeField] private Vector2 _axis;
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;

    [FormerlySerializedAs("threshold")]
    [Header("Comparison")] 
    [SerializeField] private float _threshold;
    [FormerlySerializedAs("comparison")] [SerializeField] private SpeedComparisonType _comparison;

    [FormerlySerializedAs("next")]
    [Header("Transition")] 
    [SerializeField] private CharacterState _next;
    
    public override void Enter()
    {
        if (ThresholdReached())
            Machine.TransitionTo(_next);
    }

    public override void StateFixedUpdate()
    {
        if (ThresholdReached())
            Machine.TransitionTo(_next);
    }
    
    /// <summary>
    /// Indica se o limite de velocidade, indicado pelo tipo de comparação, foi atingido
    /// </summary>
    /// <returns>se o limite de velocidade foi atingido</returns>
    private bool ThresholdReached()
    {
        return _comparison switch
        {
            SpeedComparisonType.ApproximatelyEqual => Approximately(Body.GetSpeedOnAxis(GetFinalAxis()), _threshold),
            SpeedComparisonType.ApproximatelyDifferent =>
                !Approximately(Body.GetSpeedOnAxis(GetFinalAxis()), _threshold),
            SpeedComparisonType.Higher => Body.GetSpeedOnAxis(GetFinalAxis()) > _threshold,
            SpeedComparisonType.Lower => Body.GetSpeedOnAxis(GetFinalAxis()) < _threshold,
            _ => false
        };
    }

    /// <summary>
    /// Calcula o impulso final considerando a base do vetor impulso
    /// </summary>
    /// <returns>O vetor impulso na base XY</returns>
    /// TODO: Passar esse método para a futura classe das Direções com base
    private Vector2 GetFinalAxis()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_basis);
        return _axis.x * forward + _axis.y * up;
    }

    /// <summary>
    /// Indica se dois números de ponto flutuante são próximos num raio de erro
    /// </summary>
    /// <param name="a">Primeiro número</param>
    /// <param name="b">Segundo número</param>
    /// <returns>Se os números são próximos dentro do raio de erro</returns>
    private bool Approximately(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.01f;
    }
}