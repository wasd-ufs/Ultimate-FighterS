using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Desacelera o corpo, durante o tempo desse estado como atual, conforme o vetor desaceleração e sua base
/// </summary>
public class DecelerateBehaviour : CharacterState
{
    [FormerlySerializedAs("deceleration")]
    [Header("Deceleration")]
    [SerializeField] private Vector2 _deceleration;
    // TODO: Encapsular a ideia de base numa classe ou record próprio
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;

    public override void StateFixedUpdate()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_basis);
        Body.Decelerate(forward, _deceleration.x);
        Body.Decelerate(up, _deceleration.y);
    }
}