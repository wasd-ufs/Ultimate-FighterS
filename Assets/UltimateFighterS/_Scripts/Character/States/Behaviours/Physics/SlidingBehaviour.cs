using UnityEngine;

public class SlidingBehaviour : CharacterState
{
    [Header("Sliding Variables")]
    [SerializeField] private float targetSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    public override void PhysicsProcess()
    {
        var down = body.IsOnLeftWall() 
            ? body.GetLeftWallDown() : body.IsOnRightWall() ? body.GetRightWallDown() : body.Down;
        
        body.ConvergeSpeed(down, acceleration, deceleration, targetSpeed);
    }
}