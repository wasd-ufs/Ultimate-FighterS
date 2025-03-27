using UnityEngine;

public class StretchByVelocityBehaviour : CharacterState
{
    public Vector2 axis;
    public CharacterBasis basis;
    public Vector3 stretchFactor = new Vector3(0f, 0.1f, 0f);

    private Vector3 baseScale;

    public override void Enter()
    {
        baseScale = transform.localScale;
    }
    
    public override void PhysicsProcess()
    {
        var (right, up) = GetBasis(basis);
        var finalAxis = axis.x * right + axis.y * up;
        
        var speed = Mathf.Abs(Body.GetSpeedOnAxis(finalAxis));
        transform.localScale = baseScale + stretchFactor * speed;
    }

    public override void Exit()
    {
        transform.localScale = baseScale;
    }
}