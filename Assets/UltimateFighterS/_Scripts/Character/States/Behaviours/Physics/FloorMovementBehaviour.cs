using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Move o jogador, na superfície do chão ou para sua direita (caso não esteja no chão), de forma acelerada
/// </summary>
public class FloorMovementBehaviour : CharacterState
{
    [FormerlySerializedAs("acceleration")] [Header("Movement")] [SerializeField] private float _acceleration;

    [FormerlySerializedAs("turnAcceleration")] [SerializeField] private float _turnAcceleration;
    [FormerlySerializedAs("deceleration")] [SerializeField] private float _deceleration;
    [FormerlySerializedAs("maxSpeed")] [SerializeField] private float _maxSpeed;
    [FormerlySerializedAs("overspeedDeceleration")] [SerializeField] private float _overspeedDeceleration;

    public override void StateFixedUpdate()
    {
        Vector2 forward = Body.IsOnFloor() ? Body.GetFloorRight() : Body.Right;
        float direction = Input.GetDirection().x;
        direction = direction < -0.001f ? -1 : direction > 0.001f ? 1f : 0f;

        Body.MoveSmoothly(forward,
            direction,
            _acceleration,
            _turnAcceleration,
            _deceleration,
            _maxSpeed,
            _overspeedDeceleration
        );
    }
}