using UnityEngine;
using UnityEngine.Events;

public class RotateVelocityState : CharacterState
{
    [Header("Controllability")] 
    [SerializeField] public float turnSpeed = 2f * Mathf.PI / 0.15f;
    
    public override void PhysicsProcess()
    {
        var inputDirection = input.GetDirection().x;
        if (Mathf.Approximately(inputDirection, 0f))
            return;
        
        var direction = Mathf.Sign(inputDirection);
        body.RotateVelocity(direction * turnSpeed * Time.fixedDeltaTime);
    }
}