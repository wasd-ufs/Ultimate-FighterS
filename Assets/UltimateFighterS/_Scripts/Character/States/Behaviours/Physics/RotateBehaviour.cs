using UnityEngine;

public class RotateBehaviour : CharacterState
{
    [SerializeField] public float speed;

    public override void PhysicsProcess()
    {
        var direction = Input.GetDirection();
        if (direction.sqrMagnitude < 0.01f)
            return;
        
        direction.x = direction.x < -0.01f ? 1f : direction.x > 0.01f ? -1f : 0f;
        Body.RotateVelocity(speed * direction * Time.fixedDeltaTime);
    }
}