using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Aplica gravidade no corpo, aderindo à logica de altura de pulo variável e ascensão rápida
/// </summary>
public class VariableGravityBehaviour : CharacterState
{
    [FormerlySerializedAs("heavyGravity")] [Header("Gravity")] [SerializeField] private float _heavyGravity;

    [FormerlySerializedAs("gravityRelaxationThreshold")] [SerializeField] private float _gravityRelaxationThreshold;
    [FormerlySerializedAs("normalGravity")] [SerializeField] private float _normalGravity;
    [FormerlySerializedAs("fastFallGravity")] [SerializeField] private float _fastFallGravity;

    private bool _isFastFalling;

    public override void Enter()
    {
        _isFastFalling = false;
    }

    public override void StateFixedUpdate()
    {
        _isFastFalling = ShouldFastFall();

        if (_isFastFalling && Body.IsGoingUp())
            Body.Accelerate(Body.Down * _heavyGravity);

        float gravity = _isFastFalling ? _fastFallGravity
            : Body.GetSpeedUp() > _gravityRelaxationThreshold ? _heavyGravity
            : _normalGravity;

        Body.Accelerate(Body.Down * gravity);
    }

    /// <summary>
    /// Observa se o corpo deveria entrar em queda rápida, observando se já está caindo ou se o jogador parou de querer subir
    /// </summary>
    /// <returns>Se o corpo deve entrar em queda livre</returns>
    public bool ShouldFastFall()
    {
        return _isFastFalling || Body.IsGoingDown()
                             || !(Input.IsSpecialBeingHeld() || Input.IsAttackBeingHeld() || InputPointsUp());
    }

    /// <summary>
    /// Observa se o input do jogador aponta para cima do corpo
    /// </summary>
    /// <returns>Se o input do jogador aponta para cima do corpo</returns>
    public bool InputPointsUp()
    {
        return Vector2.Dot(Input.GetDirection(), Body.Up) > 0f;
    }
}