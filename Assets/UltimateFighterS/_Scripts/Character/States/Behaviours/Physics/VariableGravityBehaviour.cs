using UnityEngine;
using UnityEngine.Serialization;

public class VariableGravityBehaviour : CharacterState
{
    [Header("Gravity")]
    [SerializeField] private float heavyGravity;
    [SerializeField] private float gravityRelaxationThreshold;
    [SerializeField] private float normalGravity;
    [SerializeField] private float fastFallGravity;
    
    private bool isFastFalling;

    public override void Enter()
    {
        isFastFalling = false;
    }

    public override void PhysicsProcess()
    {
        isFastFalling = ShouldFastFall();
        
        if (isFastFalling && Body.IsGoingUp())
            Body.Accelerate(Body.Down * heavyGravity);
        
        var gravity = isFastFalling ? fastFallGravity 
            : Body.GetSpeedUp() > gravityRelaxationThreshold ? heavyGravity
            : normalGravity;

        Body.Accelerate(Body.Down * gravity);
    }
    
    public bool ShouldFastFall() => isFastFalling || Body.IsGoingDown() 
        || !(Input.IsSpecialBeingHeld() || Input.IsAttackBeingHeld() || InputPointsUp());
    
    public bool InputPointsUp() => Vector2.Dot(Input.GetDirection(), Body.Up) > 0f;
}