using UnityEngine;

public class SlidingBehaviour : CharacterState
{
    [Header("Sliding Variables")]
    [SerializeField] private float targetSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    public override void PhysicsProcess()
    {
        var down = Body.IsOnLeftWall() 
            ? Body.GetLeftWallDown() : Body.IsOnRightWall() ? Body.GetRightWallDown() : Body.Down;
        
        Body.ConvergeSpeed(down, acceleration, deceleration, targetSpeed);
    }
}