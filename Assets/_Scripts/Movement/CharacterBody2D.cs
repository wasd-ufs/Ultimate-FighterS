using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

enum CollisionType { Floor, LeftWall, RightWall, Ceiling }

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBody2D : MonoBehaviour
{
    // Up direction and maxAngles define what is considered a floor, a left or right wall or a ceiling
    // A left direction is defined as being counterclockwise from up. Similarly, right when clockwise.
    // In case Up is Vector2.zero, all collisions will be recognized as Left Walls
    [SerializeField] private Vector2 up = Vector2.up;
    
    public Vector2 Up => up;
    public Vector2 Left => new(-up.y, up.x);
    public Vector2 Right => new(up.y, -up.x);
    public Vector2 Down => -up;
    
    [SerializeField, Range(0, 90)] private float maxFloorAngle = 45;
    [SerializeField, Range(0, 90)] private float maxCeilingAngle = 45;
    
    // Lists containing contact information from last physics frame.
    // Cannot use current physics frame has there is no guarantee all collision events have been notified
    private readonly List<ContactPoint2D> floors = new();
    private readonly List<ContactPoint2D> leftWalls = new();
    private readonly List<ContactPoint2D> rightWalls = new();
    private readonly List<ContactPoint2D> ceilings = new();
    
    // Normals calculated from lists above.
    // A zero vector means there is no contact points.
    private Vector2 floorNormal = Vector2.zero;
    private Vector2 wallNormal = Vector2.zero;
    private Vector2 leftWallNormal = Vector2.zero;
    private Vector2 rightWallNormal = Vector2.zero;
    private Vector2 ceilingNormal = Vector2.zero;

    // Lists used to buffer incoming collisions from current frame
    // Cannot use this buffer directly has there is no guarantee all collision events for this frame have been notified
    // WIP means Work In Progress 
    private readonly List<ContactPoint2D> wipFloors = new();
    private readonly List<ContactPoint2D> wipLeftWalls = new();
    private readonly List<ContactPoint2D> wipRightWalls = new();
    private readonly List<ContactPoint2D> wipCeilings = new();
    
    // Events for Collision
    // Enter events are only called if previously there were no correspondent colliders
    // A new collider entering will not call Enter Events if there was already another collider
    [SerializeField] private UnityEvent onFloorEnter;
    [SerializeField] private UnityEvent onWallEnter;
    [SerializeField] private UnityEvent onLeftWallEnter;
    [SerializeField] private UnityEvent onRightWallEnter;
    [SerializeField] private UnityEvent onCeilingEnter;

    // Exit events are only called if currently there are no correspondent colliders
    // A collider exiting will not call Exit Events if there still one or more colliders remaining
    [SerializeField] private UnityEvent onFloorExit;
    [SerializeField] private UnityEvent onWallExit;
    [SerializeField] private UnityEvent onLeftWallExit;
    [SerializeField] private UnityEvent onRightWallExit;
    [SerializeField] private UnityEvent onCeilingExit;
    
    // RigidBody2D used by movement functions.
    private Rigidbody2D body;

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
            var type = GetContactType(point);
            
            switch (type)
            {
                case CollisionType.Floor:
                    wipFloors.Add(point);
                    break;

                case CollisionType.LeftWall:
                    wipLeftWalls.Add(point);
                    break;
                
                case CollisionType.RightWall:
                    wipRightWalls.Add(point);
                    break;

                case CollisionType.Ceiling:
                    wipCeilings.Add(point);
                    break;
            }
        }
    }

    private CollisionType GetContactType(ContactPoint2D contact)
    {
        var maxFloorCosine = Mathf.Cos(maxFloorAngle * Mathf.Deg2Rad);
        var maxCeilingCosine = Mathf.Cos(maxCeilingAngle * Mathf.Deg2Rad);

        if (Vector2.Dot(up, contact.normal) > maxFloorCosine)
            return CollisionType.Floor;

        if (Vector2.Dot(-up, contact.normal) > maxCeilingCosine)
            return CollisionType.Ceiling;

        return IsNormalLeft(contact.normal) ? CollisionType.LeftWall : CollisionType.RightWall;
    }

    private void UpdateCollisions()
    {
        var lastFloorCount = floors.Count;
        var lastLeftWallCount = leftWalls.Count;
        var lastRightWallCount = rightWalls.Count;
        var lastWallCount = leftWalls.Count + rightWalls.Count;
        var lastCeilingCount = ceilings.Count;

        CascadeCollisionLists();
        CalculateNormals();
        
        var currentFloorCount = floors.Count;
        var currentLeftWallCount = leftWalls.Count;
        var currentRightWallCount = rightWalls.Count;
        var currentWallCount = leftWalls.Count + rightWalls.Count;
        var currentCeilingCount = ceilings.Count;
        
        CheckAndCallEvent(lastFloorCount, currentFloorCount, onFloorEnter, onFloorExit);
        
        CheckAndCallEvent(lastWallCount, currentWallCount, onWallEnter, onWallExit);
        CheckAndCallEvent(lastLeftWallCount, currentLeftWallCount, onLeftWallEnter, onLeftWallExit);
        CheckAndCallEvent(lastRightWallCount, currentRightWallCount, onRightWallEnter, onRightWallExit);
        
        CheckAndCallEvent(lastCeilingCount, currentCeilingCount, onCeilingEnter, onCeilingExit);
    }

    private void CheckAndCallEvent(int lastCount, int currentCount, UnityEvent enter, UnityEvent exit)
    {
        if (lastCount == 0 && currentCount > 0) enter.Invoke();
        else if (lastCount > 0 && currentCount == 0) exit.Invoke();
    }

    private void CalculateNormals()
    {
        floorNormal = AvarageNormal(GetFloorNormals());
        wallNormal = AvarageNormal(GetWallNormals());
        leftWallNormal = AvarageNormal(GetLeftWallNormals());
        rightWallNormal = AvarageNormal(GetRightWallNormals());
        ceilingNormal = AvarageNormal(GetCeilingNormals());
    }

    private Vector2 AvarageNormal(List<Vector2> vectors)
    {
        var count = vectors.Count;
        var avarage = vectors.Aggregate(Vector2.zero, (current, next) => current + next) / count;
        return avarage.normalized;
    }

    private void CascadeCollisionLists()
    {
        ClearCollisions();

        floors.AddRange(wipFloors);
        leftWalls.AddRange(wipLeftWalls);
        rightWalls.AddRange(wipRightWalls);
        ceilings.AddRange(wipCeilings);

        ClearWipCollisions();
    }

    private void ClearWipCollisions()
    {
        wipFloors.Clear();
        wipLeftWalls.Clear();
        wipRightWalls.Clear();
        wipCeilings.Clear();
    }

    private void ClearCollisions()
    {
        floors.Clear();
        leftWalls.Clear();
        rightWalls.Clear();
        ceilings.Clear();
    }

    public List<ContactPoint2D> GetFloorPoints() => floors;

    public List<ContactPoint2D> GetWallPoints()
    {
        var combinedList = new List<ContactPoint2D>(leftWalls);
        combinedList.AddRange(rightWalls);
        return combinedList;
    }
    
    public List<ContactPoint2D> GetLeftWallPoints() => leftWalls;
    public List<ContactPoint2D> GetRightWallPoints() => rightWalls;
    public List<ContactPoint2D> GetCeilingPoints() => ceilings;
    
    public List<Vector2> GetFloorNormals() => GetFloorPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetWallNormals() => GetWallPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetLeftWallNormals() => GetLeftWallPoints().ConvertAll(point => point.normal);    
    public List<Vector2> GetRightWallNormals() => GetRightWallPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetCeilingNormals() => GetCeilingPoints().ConvertAll(point => point.normal);

    public int GetFloorContactCount() => GetFloorPoints().Count;
    public int GetWallContactCount() => GetWallPoints().Count;
    public int GetLeftWallContactCount() => GetLeftWallPoints().Count;
    public int GetRightWallContactCount() => GetRightWallPoints().Count;
    public int GetCeilingContactCount() => GetCeilingPoints().Count;

    public Vector2 GetFloorNormal() => floorNormal;
    public Vector2 GetWallNormal() => wallNormal;
    public Vector2 GetLeftWallNormal() => leftWallNormal;
    public Vector2 GetRightWallNormal() => rightWallNormal;
    public Vector2 GetCeilingNormal() => ceilingNormal;

    public bool IsOnFloor() => floors.Count > 0;
    public bool IsOnWall() => IsOnLeftWall() || IsOnRightWall();
    public bool IsOnLeftWall() => leftWalls.Count > 0;
    public bool IsOnRightWall() => rightWalls.Count > 0;
    public bool IsOnCeiling() => ceilings.Count > 0;
    
    public bool IsOnFloorOnly() => IsOnFloor() && !IsOnWall() && !IsOnCeiling();
    public bool IsOnWallOnly() => !IsOnFloor() && IsOnWall() && !IsOnCeiling();
    public bool IsOnCeilingOnly() => !IsOnFloor() && !IsOnWall() && IsOnCeiling();
    public bool IsOnLeftWallOnly() => !IsOnFloor() && IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    public bool IsOnRightWallOnly() => !IsOnFloor() && !IsOnLeftWall() && IsOnRightWall() && !IsOnCeiling();

    public bool IsLeavingFloor() => IsOnFloor() && Vector2.Dot(GetFloorNormal(), GetVelocity()) > 0.001f;
    public bool IsLeavingWall() => IsOnWall() && Vector2.Dot(GetWallNormal(), GetVelocity()) > 0.001f;
    public bool IsLeavingLeftWall() => IsOnLeftWall() && Vector2.Dot(GetLeftWallNormal(), GetVelocity()) > 0.001f;
    public bool IsLeavingRightWall() => IsOnRightWall() && Vector2.Dot(GetRightWallNormal(), GetVelocity()) > 0.001f;
    public bool IsLeavingCeiling() => IsOnCeiling() && Vector2.Dot(GetCeilingNormal(), GetVelocity()) > 0.001f;
    
    public bool IsOnFloorStable() => IsOnFloor() && Vector2.Dot(GetFloorNormal(), GetVelocity()) <= 0.001f;
    public bool IsOnWallStable() => IsOnWall() && Vector2.Dot(GetWallNormal(), GetVelocity()) <= 0.001f;
    public bool IsOnLeftWallStable() => IsOnLeftWall() && Vector2.Dot(GetLeftWallNormal(), GetVelocity()) <= 0.001f;
    public bool IsOnRightWallStable() => IsOnRightWall() && Vector2.Dot(GetRightWallNormal(), GetVelocity()) <= 0.001f;
    public bool IsOnCeilingStable() => IsOnCeiling() && Vector2.Dot(GetCeilingNormal(), GetVelocity()) <= 0.001f;

    public bool IsGoingUp() => Vector2.Dot(body.velocity, up) > 0.001f;
    public bool IsGoingDown() => Vector2.Dot(body.velocity, Down) > 0.001f;
    public bool HasNoVerticalSpeed() => Mathf.Approximately(0f, Vector2.Dot(body.velocity, up));
    public bool IsGoingLeft() => Vector2.Dot(body.velocity, Left) > 0.001f;
    public bool IsGoingRight() => Vector2.Dot(body.velocity, Right) > 0.001f;
    public bool HasNoHorizontalSpeed() => Mathf.Approximately(0f, Vector2.Dot(body.velocity, Left));

    public Vector2 GetVelocity() => body.velocity;
    public float GetSpeedOnAxis(Vector2 axis) => Vector2.Dot(axis, body.velocity);

    private bool IsNormalLeft(Vector2 normal) => Vector2.Dot(normal, Left) > 0.001f;
    private bool IsNormalRight(Vector2 normal) => Vector2.Dot(normal, Right) > 0.001f;
}
