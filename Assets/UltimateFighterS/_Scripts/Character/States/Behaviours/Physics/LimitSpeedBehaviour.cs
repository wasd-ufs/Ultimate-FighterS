using System;
using UnityEngine;

public class LimitSpeedBehaviour : CharacterState
{
    [Header("Axis")]
    [SerializeField] public Vector2 axis;
    [SerializeField] public float maxSpeed;
    [SerializeField] public CharacterBasis basis;

    public override void PhysicsProcess()
    {
        var (forward, up) = GetBasis(basis);
        var axis = forward * this.axis.x + up * this.axis.y;
        body.LimitSpeed(axis, maxSpeed);
    }
}