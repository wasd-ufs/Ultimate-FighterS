using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Rotaciona o corpo numa dada velocidade angular no sentido horário ou anti-horário segundo a componente X do input
/// </summary>
public class RotateBehaviour : CharacterState
{
    [FormerlySerializedAs("speed")] [SerializeField] private float _speed;

    public override void StateFixedUpdate()
    {
        Vector2 direction = Input.GetDirection();
        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.x = direction.x < -0.01f ? 1f : direction.x > 0.01f ? -1f : 0f;
        Body.RotateVelocity(_speed * direction * Time.fixedDeltaTime);
    }
}