using UnityEngine;

public class RotateBehaviour : CharacterState
{
    [SerializeField] public float speed;

    public override void PhysicsProcess()
    {
        var direction = input.GetDirection().x;
        direction = direction < -0.01f ? 1f : direction > 0.01f ? -1f : 0f;
        body.RotateVelocity(direction * speed * Time.fixedDeltaTime);
    }
}