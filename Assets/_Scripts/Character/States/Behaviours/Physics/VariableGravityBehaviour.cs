using UnityEngine;

public class VariableGravityBehaviour : CharacterState
{
    [Header("Gravity")]
    [SerializeField] private float normalGravity;
    [SerializeField] private float fastFallGravity;
    
    private bool isFastFalling;

    public override void Enter()
    {
        isFastFalling = body.IsGoingDown() || !(input.IsSpecialBeingHeld() || InputPointsUp());
    }

    public override void PhysicsProcess()
    {
        isFastFalling = isFastFalling || body.IsGoingDown() || !(input.IsSpecialBeingHeld() || InputPointsUp());
        
        var gravity = isFastFalling ? fastFallGravity : normalGravity;
        var down = body.Down;
        
        body.Accelerate(down * gravity);
    }

    public bool InputPointsUp() => Vector2.Dot(input.GetDirection(), body.Up) > 0f;
}