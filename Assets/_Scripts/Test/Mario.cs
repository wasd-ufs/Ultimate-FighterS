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
        if (body.IsOnFloor() && WantsToJump())
            Jump();
    }

    private void FixedUpdate()
    {
        //if (body.IsOnFloorStable())
        //    body.Up = body.GetFloorNormal();
        
        body.MoveSmoothly(body.IsOnFloor()? body.GetFloorRight() : body.Right, GetDirection(), acceleration, turnAcceleration, deceleration, maxSpeed);
        
        if (!body.IsOnFloor()) 
            ApplyGravity();
        
        UpdateFastFalling();
    }
    
    private void ApplyGravity()
    {
        var chosenGravity = isFastFalling ? fallGravity : gravity;
        body.Accelerate(chosenGravity * body.Down);
        body.LimitSpeed(body.Down, maxFallSpeed);
    }

    private void Jump()
    {
        body.SetSpeed(body.Up, jumpImpulse);
        isFastFalling = false;
    }

    private void UpdateFastFalling()
    {
        isFastFalling = isFastFalling || body.IsGoingDown() || !IsHoldingJump();
    }

    private bool IsTurningAround(float direction)
    {
        var velocity = body.Velocity;
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
