using UnityEngine;

public class DecelerateBehaviour : CharacterState
{
    [Header("Deceleration")]
    [SerializeField] public Vector2 deceleration;
    [SerializeField] public CharacterBasis basis;

    public override void PhysicsProcess()
    {
        var (forward, up) = GetBasis(basis);
        body.Decelerate(forward, deceleration.x);
        body.Decelerate(up, deceleration.y);
    }
}