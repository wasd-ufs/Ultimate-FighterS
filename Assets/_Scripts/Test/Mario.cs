using UnityEngine;

[RequireComponent(typeof(CharacterBody2D))]
public class Mario : MonoBehaviour
{
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode jump = KeyCode.Space;
    
    [SerializeField] private float maxSpeed = 5.85f;
    [SerializeField] private float acceleration = 8.3475f;
    [SerializeField] private float deceleration = 11.4f;
    [SerializeField] private float turnAcceleration = 22.8375f;

    [SerializeField] private float jumpImpulse = 25f;
    [SerializeField] private float gravity = 35f;
    [SerializeField] private float fallGravity = 125f;
    [SerializeField] private float maxFallSpeed = 16.5f;
    
    private CharacterBody2D body;
    private bool isFastFalling;
    
    private void Start()
    {
        body = GetComponent<CharacterBody2D>();
    }

    private void Update()
    {
        Debug.Log(body.IsOnFloor() && !body.IsRising());
        
        if (body.IsOnFloor() && !body.IsRising() && WantsToJump())
            Jump();
    }

    private void FixedUpdate()
    {
        body.MoveSmoothly(Vector2.right, GetDirection(), acceleration, turnAcceleration, deceleration, maxSpeed);
        ApplyGravity();
        UpdateFastFalling();
    }

    private void Move()
    {
        var direction = GetDirection();
        if (Mathf.Approximately(direction, 0f))
            body.Decelerate(Vector2.right, deceleration);

        else if (IsTurningAround(direction))
            body.Accelerate(direction * turnAcceleration * Vector2.right);
        
        else
            body.Accelerate(direction * acceleration * Vector2.right);
        
        body.ClampSpeed(Vector2.right, -maxSpeed, maxSpeed);
    }
    
    private void ApplyGravity()
    {
        var chosenGravity = isFastFalling ? fallGravity : gravity;
        body.Accelerate(chosenGravity * Vector2.down);
        body.LimitSpeed(Vector2.down, maxFallSpeed);
    }

    private void Jump()
    {
        body.ApplyImpulse(jumpImpulse * Vector2.up);
        isFastFalling = false;
    }

    private void UpdateFastFalling()
    {
        isFastFalling = isFastFalling || body.IsFalling() || !IsHoldingJump();
    }

    private bool IsTurningAround(float direction)
    {
        var velocity = body.GetVelocity();
        return (velocity.x > 0f && direction < 0f) || (velocity.x < 0f && direction > 0f);
    }

    private float GetDirection()
    {
        var rightWeight = Input.GetKey(right) ? 1f : 0f;
        var leftWeight = Input.GetKey(left) ? 1f : 0f;
        return rightWeight - leftWeight;
    }

    private bool WantsToJump()
        => Input.GetKeyDown(jump);

    private bool IsHoldingJump()
        => Input.GetKey(jump);
}
