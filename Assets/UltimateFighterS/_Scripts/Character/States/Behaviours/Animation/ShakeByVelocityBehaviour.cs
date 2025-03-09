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
        originalPos = Body.transform.localPosition;
        
        shakeDirection = Body.Velocity;
        if (shakeDirection.sqrMagnitude < 0.01f)
            shakeDirection = Body.Right;
        
        speedForce = Mathf.Sqrt(shakeDirection.magnitude);
        shakeDirection.Normalize();
    }

    public override void PhysicsProcess()
    {
        Shake();
    }

    private void Shake()
    {
        Body.transform.localPosition = originalPos + (Vector3)shakeDirection * (speedForce * scaledForce * Random.Range(0.5f, 1f));
        shakeDirection *= -1;
    }

    public override void Exit()
    {
        Body.transform.localPosition = originalPos;
    }
}