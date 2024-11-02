using UnityEngine;

public class RotateBehaviour : CharacterState
{
    [SerializeField] public float speed;

    public override void PhysicsProcess()
    {
        var direction = input.GetDirection();
        
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        body.RotateVelocity(angle);
        
        direction.x = direction.x < -0.01f ? 1f : direction.x > 0.01f ? -1f : 0f;
        body.RotateVelocity(direction * speed * Time.fixedDeltaTime);
    }
}