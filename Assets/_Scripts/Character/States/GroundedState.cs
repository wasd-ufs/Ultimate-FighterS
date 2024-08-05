using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : CharacterState
{
    [Header("Transitions")]
    [SerializeField] private CharacterState airborne;

    [Header("Horizontal")] 
    [SerializeField] private float acceleration;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float overspeedDeceleration;
    
    [Header("Vertical")]
    [SerializeField] private float jumpForce;
    
    public override void Process()
    {
        if (input.IsSpecialJustPressed())
        {
            body.SetSpeed(body.Up, jumpForce);
            machine.TransitionTo(airborne);
        }
    }

    public override void PhysicsProcess()
    {
        body.MoveSmoothly(body.GetFloorRight(), input.GetDirection().x, acceleration, turnAcceleration, deceleration, maxSpeed, overspeedDeceleration);
        if (!body.IsOnFloor()) machine.TransitionTo(airborne);
    }
}

