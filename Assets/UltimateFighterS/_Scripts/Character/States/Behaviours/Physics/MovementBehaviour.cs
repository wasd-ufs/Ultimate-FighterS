using UnityEngine;

public class MovementBehaviour : CharacterState
{
    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float overspeedDeceleration;

    public override void PhysicsProcess()
    {
        var forward = Body.IsOnFloor() ? Body.GetFloorRight() : Body.Right;
        var direction = Input.GetDirection().x;
        direction = direction < -0.001f ? -1 : direction > 0.001f ? 1f : 0f;
        
        Body.MoveSmoothly(forward, 
            direction,
            acceleration,
            turnAcceleration,
            deceleration,
            maxSpeed,
            overspeedDeceleration
        );
    }
}