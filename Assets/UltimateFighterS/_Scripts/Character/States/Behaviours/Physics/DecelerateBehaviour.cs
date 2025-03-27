using UnityEngine;

public class DecelerateBehaviour : CharacterState
{
    [Header("Deceleration")]
    [SerializeField] public Vector2 deceleration;
    [SerializeField] public CharacterBasis basis;

    public override void PhysicsProcess()
    {
        var (forward, up) = GetBasis(basis);
        Body.Decelerate(forward, deceleration.x);
        Body.Decelerate(up, deceleration.y);
    }
}