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
        isFastFalling = !(input.IsAttackBeingHeld() || input.IsSpecialBeingHeld()) || input.GetDirection().sqrMagnitude < 0.01f;
    }

    public override void PhysicsProcess()
    {
        isFastFalling = ShouldFastFall();
        if (isFastFalling && body.IsGoingUp())
            body.Accelerate(body.Down * fastFallGravity * Mathf.Pow(body.GetSpeedUp() / gravityRelaxationThreshold, 2));
        
        var gravity = isFastFalling ? fastFallGravity 
            : body.GetSpeedUp() > gravityRelaxationThreshold ? heavyGravity
            : normalGravity;

        body.Accelerate(body.Down * gravity);
    }
    
    public bool ShouldFastFall() => body.IsGoingDown() 
        || !(input.IsSpecialBeingHeld() || input.IsAttackBeingHeld() || InputPointsUp());
    
    public bool InputPointsUp() => Vector2.Dot(input.GetDirection(), body.Up) > 0f;
}