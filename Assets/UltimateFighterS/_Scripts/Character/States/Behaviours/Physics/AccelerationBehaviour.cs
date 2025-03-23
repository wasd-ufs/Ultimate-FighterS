using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Acelera o corpo, durante o tempo desse estado como atual, a favor do vetor aceleração em sua base
/// </summary>
public class AccelerateBehaviour : CharacterState
{
    [FormerlySerializedAs("acceleration")] [Header("Acceleration")] [SerializeField]
    private Vector2 _acceleration;

    // TODO: Encapsular a ideia de base numa classe ou record próprio
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;

    public override void StateFixedUpdate()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_basis);
        Body.Accelerate(_acceleration.x * forward + _acceleration.y * up);
    }
}