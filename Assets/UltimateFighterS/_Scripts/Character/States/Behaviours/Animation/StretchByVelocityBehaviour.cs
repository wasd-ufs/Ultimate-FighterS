using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Estica o corpo do personagem, alongando no eixo e contraindo no eixo perpendicular, baseado na velocidade atual e no fator de esticamento
/// Ao sair do estado, retorna a escala do corpo ao valor original de antes de entrar no estado
/// </summary>
public class StretchByVelocityBehaviour : CharacterState
{
    [FormerlySerializedAs("axis")] [SerializeField] private Vector2 _axis;
    // TODO: Encapsular a ideia de base numa classe ou record próprio
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;
    [FormerlySerializedAs("stretchFactor")] [SerializeField] private Vector3 _stretchFactor = new(0f, 0.1f, 0f);

    private Vector3 _baseScale;

    public override void Enter()
    {
        _baseScale = transform.localScale;
    }

    public override void StateFixedUpdate()
    {
        (Vector2 right, Vector2 up) = GetBasis(_basis);
        Vector2 finalAxis = _axis.x * right + _axis.y * up;

        float speed = Mathf.Abs(Body.GetSpeedOnAxis(finalAxis));
        transform.localScale = _baseScale + _stretchFactor * speed;
    }

    public override void Exit()
    {
        transform.localScale = _baseScale;
    }
}