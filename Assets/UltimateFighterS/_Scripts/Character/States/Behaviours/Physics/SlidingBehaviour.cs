using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Faz o jogador deslisar na parede, ou ir para baixo caso não esteja na parede, e convergir para uma velocidade alvo, acelerando caso esteja abaixo ou desacelerando caso esteja acima
/// </summary>
public class SlidingBehaviour : CharacterState
{
    [FormerlySerializedAs("targetSpeed")] [Header("Sliding Variables")] [SerializeField]
    private float _targetSpeed;

    [FormerlySerializedAs("acceleration")] [SerializeField] private float _acceleration;
    [FormerlySerializedAs("deceleration")] [SerializeField] private float _deceleration;

    public override void StateFixedUpdate()
    {
        Vector2 down = Body.IsOnLeftWall()
            ? Body.GetLeftWallDown()
            : Body.IsOnRightWall()
                ? Body.GetRightWallDown()
                : Body.Down;

        Body.ConvergeSpeed(down, _acceleration, _deceleration, _targetSpeed);
    }
}