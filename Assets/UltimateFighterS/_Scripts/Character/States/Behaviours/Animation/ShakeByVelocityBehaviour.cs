using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeByVelocityBehaviour : CharacterState
{
    [SerializeField] private float scaledForce;
    
    private Vector3 originalPos;
    private Vector2 shakeDirection;
    private float speedForce;
    
    public override void Enter()
    {
        originalPos = body.transform.localPosition;
        
        shakeDirection = body.Velocity;
        if (shakeDirection.sqrMagnitude < 0.01f)
            shakeDirection = body.Right;
        
        speedForce = Mathf.Sqrt(shakeDirection.magnitude);
        shakeDirection.Normalize();
    }

    public override void PhysicsProcess()
    {
        Shake();
    }

    private void Shake()
    {
        body.transform.localPosition = originalPos + (Vector3)shakeDirection * (speedForce * scaledForce * Random.Range(0.5f, 1f));
        shakeDirection *= -1;
    }

    public override void Exit()
    {
        body.transform.localPosition = originalPos;
    }
}