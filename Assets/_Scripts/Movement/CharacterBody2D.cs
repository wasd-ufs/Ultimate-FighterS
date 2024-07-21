using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBody2D : MonoBehaviour
{
    [SerializeField] private Vector2 upDirection = Vector2.up;
    [SerializeField, Range(0, 90)] private float maxFloorAngle = 45;
    [SerializeField, Range(0, 90)] private float maxCeilingAngle = 45;

    private readonly List<CharacterCollision2D> lastFloors = new List<CharacterCollision2D>();
    private readonly List<CharacterCollision2D> lastWalls = new List<CharacterCollision2D>();
    private readonly List<CharacterCollision2D> lastCeilings = new List<CharacterCollision2D>();

    private readonly List<CharacterCollision2D> floors = new List<CharacterCollision2D>();
    private readonly List<CharacterCollision2D> walls = new List<CharacterCollision2D>();
    private readonly List<CharacterCollision2D> ceilings = new List<CharacterCollision2D>();

    [SerializeField] private UnityEvent onFloorEnter;
    [SerializeField] private UnityEvent onWallEnter;
    [SerializeField] private UnityEvent onCeilingEnter;

    [SerializeField] private UnityEvent onFloorExit;
    [SerializeField] private UnityEvent onWallExit;
    [SerializeField] private UnityEvent onCeilingExit;

    private Rigidbody2D body;
    private bool updatedCollisions;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.sleepMode = RigidbodySleepMode2D.NeverSleep;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void MoveAndClamp(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x =>
            {
                if (Mathf.Approximately(direction, 0f))
                    x = Mathf.MoveTowards(x, 0f, deceleration * Time.fixedDeltaTime);
                
                else if ((x > 0 && direction < 0) || (x < 0 && direction > 0))
                    x += direction * turnAcceleration * Time.fixedDeltaTime;

                else
                    x += direction * acceleration * Time.fixedDeltaTime;

                x = Mathf.Clamp(x, -maxSpeed, maxSpeed);
                return x;
            }
        );
    }
    
    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            AccelerateUntil(axis * direction, turnAcceleration, maxSpeed);
        
        else 
            AccelerateUntil(axis * direction, acceleration, maxSpeed);
        
        DecelerateUntil(axis, deceleration, maxSpeed);
    }
    
    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed, float overspeedDeceleration)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            AccelerateUntil(axis * direction, turnAcceleration, maxSpeed);
        
        else 
            AccelerateUntil(axis * direction, acceleration, maxSpeed);
        
        DecelerateUntil(axis, overspeedDeceleration, maxSpeed);
    }

    public void Accelerate(Vector2 acceleration)
    {
        body.velocity += acceleration * Time.fixedDeltaTime;
    }

    public void Accelerate(float acceleration)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => magnitude + acceleration * Time.fixedDeltaTime
        );
    }

    public void AccelerateUntil(Vector2 axis, float acceleration, float maxSpeed)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => (x > maxSpeed) ? x : Mathf.MoveTowards(x, maxSpeed, acceleration * Time.fixedDeltaTime)
        );
    }

    public void AccelerateUntil(float acceleration, float maxSpeed)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => (magnitude > maxSpeed) ? magnitude : Mathf.MoveTowards(magnitude, maxSpeed, magnitude * Time.fixedDeltaTime)
        );
    }
    
    public void Decelerate(Vector2 axis, float deceleration)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => Mathf.MoveTowards(x, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void Decelerate(float deceleration)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => Mathf.MoveTowards(magnitude, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void DecelerateUntil(Vector2 axis, float deceleration, float minSpeed)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => (x < minSpeed) ? x : Mathf.MoveTowards(x, minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }
    
    public void DecelerateUntil(float deceleration, float minSpeed)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => (magnitude < minSpeed) ? magnitude : Mathf.MoveTowards(magnitude, minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        body.velocity += impulse;
    }

    public void ClampSpeed(Vector2 axis, float minimum, float maximum)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => Mathf.Clamp(x, minimum, maximum)
        );
    }

    public void ClampSpeed(float minimum, float maximum)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => Mathf.Clamp(magnitude, minimum, maximum)
        );
    }

    public void LimitSpeed(Vector2 axis, float maximum)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => Mathf.Min(x, maximum)
        );
    }

    public void LimitSpeed(float maximum)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity,
            magnitude => Mathf.Min(magnitude, maximum)
        );
    }

    public void ConvergeSpeed(Vector2 axis, float acceleration, float deceleration, float speed)
    {
        AccelerateUntil(axis, acceleration, speed);
        DecelerateUntil(axis, deceleration, speed);
    }

    public void ConvergeSpeed(Vector2 axis, float acceleration, float speed)
    {
        ConvergeSpeed(axis, acceleration, acceleration, speed);
    }

    public void ConvergeSpeed(float acceleration, float deceleration, float speed)
    {
        AccelerateUntil(acceleration, speed);
        DecelerateUntil(deceleration, speed);
    }

    public void ConvergeSpeed(float acceleration, float speed)
    {
        ConvergeSpeed(acceleration, acceleration, speed);
    }

    public void SetSpeed(Vector2 axis, float speed)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity,
            x => speed
        );
    }

    public void SetVelocity(Vector2 speed)
    {
        body.velocity = speed;
    }
    
    public void ModifyVelocity(Func<Vector2, Vector2> modification)
    {
        var velocity = body.velocity;
        velocity = modification(velocity);
        body.velocity = velocity;
    }
    
    public void ModifyVelocity(Func<float, float> modificationX, Func<float, float> modificationY)
    {
        var velocity = body.velocity;
        velocity.x = modificationX(velocity.x);
        velocity.y = modificationY(velocity.y);
        body.velocity = velocity;
    }

    public void ModifyVelocityX(Func<float, float> modification)
    {
        ModifyVelocity(modification, y => y);
    }

    public void ModifyVelocityY(Func<float, float> modification)
    {
        ModifyVelocity(x => x, modification);
    }

    public void ModifyVelocityOnAxis(Vector2 axis, Func<float, float> modification)
    {
        body.velocity = VectorUtils.OnAxis(axis, body.velocity, modification);
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification)
    {
        body.velocity = VectorUtils.OnMagnitude(body.velocity, modification);
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification, Vector2 defaultValue)
    {
        if (Mathf.Approximately(body.velocity.sqrMagnitude, 0f))
            body.velocity = defaultValue;
        
        else
            ModifyVelocityMagnitude(modification);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        AddCollision(other);
    }

    private void FixedUpdate()
    {
        UpdateCollisions();
    }

    private void AddCollision(Collision2D other)
    {
        var contacts = new List<ContactPoint2D>();
        other.GetContacts(contacts);

        foreach (var point in contacts)
        {
            var collision = new CharacterCollision2D(point, GetContactType);

            switch (collision.type)
            {
                case CollisionType.Floor:
                    floors.Add(collision);
                    break;

                case CollisionType.Wall:
                    walls.Add(collision);
                    break;

                case CollisionType.Ceiling:
                    ceilings.Add(collision);
                    break;
            }
        }
    }

    private CollisionType GetContactType(ContactPoint2D contact)
    {
        var maxFloorCosine = Mathf.Cos(maxFloorAngle * Mathf.Deg2Rad);
        var maxCeilingCosine = Mathf.Cos(maxCeilingAngle * Mathf.Deg2Rad);

        if (Vector2.Dot(upDirection, contact.normal) > maxFloorCosine)
            return CollisionType.Floor;

        if (Vector2.Dot(-upDirection, contact.normal) > maxCeilingCosine)
            return CollisionType.Ceiling;

        return CollisionType.Wall;
    }

    private void UpdateCollisions()
    {
        var lastFloorCount = lastFloors.Count;
        var lastWallCount = lastWalls.Count;
        var lastCeilingCount = lastCeilings.Count;

        CascadeCollisionLists();

        // onEnter
        if (lastFloorCount == 0 && lastFloors.Count > 0)
            onFloorEnter.Invoke();

        if (lastWallCount == 0 && lastWalls.Count > 0)
            onWallEnter.Invoke();

        if (lastCeilingCount == 0 && lastCeilings.Count > 0)
            onCeilingEnter.Invoke();

        // onExit
        if (lastFloorCount > 0 && lastFloors.Count == 0)
            onFloorExit.Invoke();

        if (lastWallCount > 0 && lastWalls.Count == 0)
            onWallExit.Invoke();

        if (lastCeilingCount > 0 && lastCeilings.Count == 0)
            onCeilingExit.Invoke();
    }

    private void CascadeCollisionLists()
    {
        ClearLastCollisions();

        lastFloors.AddRange(floors);
        lastWalls.AddRange(walls);
        lastCeilings.AddRange(ceilings);

        ClearCollisions();
    }

    private void ClearCollisions()
    {
        floors.Clear();
        walls.Clear();
        ceilings.Clear();
    }

    private void ClearLastCollisions()
    {
        lastFloors.Clear();
        lastWalls.Clear();
        lastCeilings.Clear();
    }

    public List<CharacterCollision2D> GetFloorPoints() => lastFloors;
    public List<CharacterCollision2D> GetWallPoints() => lastWalls;
    public List<CharacterCollision2D> GetCeilingPoints() => lastCeilings;
    
    public List<Vector2> GetFloorNormals() => lastFloors.ConvertAll(point => point.normal);
    public List<Vector2> GetWallNormals() => lastWalls.ConvertAll(point => point.normal);
    public List<Vector2> GetCeilingNormals() => lastCeilings.ConvertAll(point => point.normal);

    public int GetFloorContactCount() => lastFloors.Count;
    public int GetWallContactCount() => lastFloors.Count;
    public int GetCeilingContactCount() => lastFloors.Count;

    public Vector2 GetAverageFloorNormal() =>
        GetFloorPoints().ConvertAll(point => point.normal)
            .Aggregate(Vector2.zero, (current, next) => current + next) / GetFloorContactCount();
    
    public Vector2 GetAverageWallNormal() =>
        GetWallPoints().ConvertAll(point => point.normal)
            .Aggregate(Vector2.zero, (current, next) => current + next) / GetWallContactCount();
    
    public Vector2 GetCeilingFloorNormal() =>
        GetCeilingPoints().ConvertAll(point => point.normal)
            .Aggregate(Vector2.zero, (current, next) => current + next) / GetCeilingContactCount();

    public bool IsOnFloor() => lastFloors.Count > 0;
    public bool IsOnWall() => lastWalls.Count > 0;
    public bool IsOnCeiling() => lastCeilings.Count > 0;

    public bool IsOnFloorOnly() => IsOnFloor() && !IsOnWall() && !IsOnCeiling();
    public bool IsOnWallOnly() => !IsOnFloor() && IsOnWall() && !IsOnCeiling();
    public bool IsOnCeilingOnly() => !IsOnFloor() && !IsOnWall() && IsOnCeiling();

    public bool IsLeavingFloor() => IsOnFloor() && IsRising();
    public bool IsLeavingCeiling() => IsOnCeiling() && IsFalling();

    public bool IsRising() => Vector2.Dot(body.velocity, upDirection) > 0.001f;
    public bool IsFalling() => Vector2.Dot(body.velocity, -upDirection) > 0.001f;
    public bool HasNoVerticalSpeed() => Mathf.Approximately(0f, Vector2.Dot(body.velocity, upDirection));

    public Vector2 GetVelocity() => body.velocity;
    public float GetSpeedOnAxis(Vector2 axis) => Vector2.Dot(axis, body.velocity);
}
