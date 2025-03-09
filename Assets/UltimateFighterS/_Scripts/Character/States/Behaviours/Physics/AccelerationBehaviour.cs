using UnityEngine;

public class AccelerateBehaviour : CharacterState
{
    [Header("Acceleration")]
    [SerializeField] public Vector2 acceleration;
    [SerializeField] public CharacterBasis basis;

    public override void PhysicsProcess()
    {
        var (forward, up) = GetBasis(basis);
        Body.Accelerate(acceleration.x * forward + acceleration.y * up);
    }
}