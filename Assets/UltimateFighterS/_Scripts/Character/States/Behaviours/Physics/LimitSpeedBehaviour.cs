using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Garante, durante a execução do estado, que a velocidade do corpo não ultrapassará um valor máximo num dado eixo
/// </summary>
public class LimitSpeedBehaviour : CharacterState
{
    [FormerlySerializedAs("axis")]
    [Header("Axis")] 
    [SerializeField] private Vector2 _axis;
    [FormerlySerializedAs("basis")] [SerializeField] private CharacterBasis _basis;

    [FormerlySerializedAs("maxSpeed")] [SerializeField] private float _maxSpeed;

    public override void StateFixedUpdate()
    {
        (Vector2 forward, Vector2 up) = GetBasis(_basis);
        Vector2 axis = forward * this._axis.x + up * this._axis.y;
        Body.LimitSpeed(axis, _maxSpeed);
    }
}