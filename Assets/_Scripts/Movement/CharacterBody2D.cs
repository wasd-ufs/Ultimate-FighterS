using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

enum CollisionType { Floor, LeftWall, RightWall, Ceiling }

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBody2D : MonoBehaviour
{
    const float ErrorWindow = 0.05f;
    
    // Up direction and maxAngles define what is considered a floor, a left or right wall or a ceiling
    // A left direction is defined as being counterclockwise from up. Similarly, right when clockwise.
    // In case Up is Vector2.zero, all collisions will be recognized as Left Walls
    [SerializeField] private Vector2 up = Vector2.up;

    public Vector2 Up
    {
        set
        {
            var newUp = value.normalized;
            RotateVelocity(newUp);
            up = newUp;
        }
        get => up;
    }
    
    public Vector2 Left => new(-up.y, up.x);
    public Vector2 Right => new(up.y, -up.x);
    public Vector2 Down => -up;
    
    [SerializeField, Range(0, 90)] private float maxFloorAngle = 45;
    [SerializeField] private float maxSnapLength = 0.5f;

    public readonly List<Vector2> Floors = new();
    public readonly List<Vector2> LeftWalls = new();
    public readonly List<Vector2> RightWalls = new();
    public readonly List<Vector2> Ceilings = new();
    
    // Normals calculated from lists above.
    // A zero vector means there is no contact points.
    public Vector2 FloorNormal { get; private set; } = Vector2.zero;
    public Vector2 LeftWallNormal { get; private set; } = Vector2.zero;
    public Vector2 RightWallNormal { get; private set; } = Vector2.zero;
    public Vector2 CeilingNormal { get; private set; } = Vector2.zero;
    
    // Events for Collision
    // Enter events are only called if previously there were no correspondent colliders
    // A new collider entering will not call Enter Events if there was already another collider
    public UnityEvent<Vector2> onFloorEnter;
    public UnityEvent<Vector2> onWallEnter;
    public UnityEvent<Vector2> onLeftWallEnter;
    public UnityEvent<Vector2> onRightWallEnter;
    public UnityEvent<Vector2> onCeilingEnter;

    // Exit events are only called if currently there are no correspondent colliders
    // A collider exiting will not call Exit Events if there still one or more colliders remaining
    public UnityEvent<Vector2> onFloorExit;
    public UnityEvent<Vector2> onWallExit;
    public UnityEvent<Vector2> onLeftWallExit;
    public UnityEvent<Vector2> onRightWallExit;
    public UnityEvent<Vector2> onCeilingExit;
    
    // RigidBody2D used by movement functions.
    private readonly List<Collider2D> colliders = new();
    private Rigidbody2D body;
    private bool skipSnapping;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.GetAttachedColliders(colliders);
        body.sleepMode = RigidbodySleepMode2D.NeverSleep;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void MoveAndClamp(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            Accelerate(axis * (direction * turnAcceleration));
        
        else 
            Accelerate(axis * (direction * acceleration));
        
        ClampSpeed(axis, -maxSpeed, maxSpeed);
    }
    
    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            AccelerateUntil(axis * Mathf.Sign(direction), turnAcceleration, maxSpeed);
        
        else 
            AccelerateUntil(axis * Mathf.Sign(direction), acceleration, maxSpeed);
        
        DecelerateUntil(axis, deceleration, maxSpeed);
    }
    
    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed, float overspeedDeceleration)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            AccelerateUntil(axis * Mathf.Sign(direction), turnAcceleration, maxSpeed);
        
        else 
            AccelerateUntil(axis * Mathf.Sign(direction), acceleration, maxSpeed);
        
        DecelerateUntil(axis, overspeedDeceleration, maxSpeed);
    }
    
    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration, float deceleration,
        float maxSpeed, float overspeedDeceleration, float maxOverspeed)
    {
        if (Mathf.Approximately(direction, 0f))
            Decelerate(axis, deceleration);

        else if ((GetSpeedOnAxis(axis) > 0f && direction < 0f) || (GetSpeedOnAxis(axis) < 0f && direction > 0f))
            AccelerateUntil(axis * Mathf.Sign(direction), turnAcceleration, maxSpeed);
        
        else 
            AccelerateUntil(axis * Mathf.Sign(direction), acceleration, maxSpeed);
        
        DecelerateUntil(axis, overspeedDeceleration, maxOverspeed);
    }

    public void Accelerate(Vector2 acceleration)
    {
        ModifyVelocity(velocity => velocity + acceleration * Time.fixedDeltaTime);
    }

    public void Accelerate(float acceleration)
    {
        ModifyVelocityMagnitude(
            magnitude => magnitude + acceleration * Time.fixedDeltaTime
        );
    }
    
    public void Accelerate(float acceleration, Vector2 defaultDirection)
    {
        ModifyVelocityMagnitude(
            magnitude => magnitude + acceleration * Time.fixedDeltaTime,
            defaultDirection.normalized * acceleration * Time.fixedDeltaTime
        );
    }

    public void AccelerateUntil(Vector2 axis, float acceleration, float maxSpeed)
    {
        ModifyVelocityOnAxis(axis, 
            x => (x >= maxSpeed) ? x : Mathf.MoveTowards(x, maxSpeed, acceleration * Time.fixedDeltaTime)
        );
    }

    public void AccelerateUntil(float acceleration, float maxSpeed)
    {
        ModifyVelocityMagnitude(
            magnitude => (magnitude > maxSpeed) ? magnitude : Mathf.MoveTowards(magnitude, maxSpeed, magnitude * Time.fixedDeltaTime)
        );
    }
    
    public void Decelerate(Vector2 axis, float deceleration)
    {
        ModifyVelocityOnAxis(axis, 
            x => Mathf.MoveTowards(x, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void Decelerate(float deceleration)
    {
        ModifyVelocityMagnitude(
            magnitude => Mathf.MoveTowards(magnitude, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void DecelerateUntil(Vector2 axis, float deceleration, float minSpeed)
    {
        ModifyVelocityOnAxis(axis, 
            x => (Mathf.Abs(x) <= minSpeed) ? x : Mathf.Sign(x) * Mathf.MoveTowards(Mathf.Abs(x), minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }
    
    public void DecelerateUntil(float deceleration, float minSpeed)
    {
        ModifyVelocityMagnitude(
            magnitude => (magnitude < minSpeed) ? magnitude : Mathf.MoveTowards(magnitude, minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        ModifyVelocity((velocity) => velocity + impulse);
    }

    public void ClampSpeed(Vector2 axis, float minimum, float maximum)
    {
        ModifyVelocityOnAxis(axis,
            x => Mathf.Clamp(x, minimum, maximum)
        );
    }

    public void ClampSpeed(float minimum, float maximum)
    {
        ModifyVelocityMagnitude(
            magnitude => Mathf.Clamp(magnitude, minimum, maximum)
        );
    }

    public void LimitSpeed(Vector2 axis, float maximum)
    {
        ModifyVelocityOnAxis(axis,
            x => Mathf.Min(x, maximum)
        );
    }

    public void LimitSpeed(float maximum)
    {
        ModifyVelocityMagnitude(
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
        ModifyVelocity((velocity) => VectorUtils.OnAxis(axis, velocity,
            x => speed
        ));
    }

    public void SetVelocity(Vector2 speed)
    {
        body.velocity = speed;
        skipSnapping = !IsSurfaceStable(FloorNormal);
    }
    
    public void ModifyVelocity(Func<Vector2, Vector2> modification)
    {
        SetVelocity(modification(body.velocity));
    }
    
    public void ModifyVelocity(Func<float, float> modificationX, Func<float, float> modificationY)
    {
        ModifyVelocity((velocity) => new Vector2(modificationX(velocity.x), modificationY(velocity.y)));
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
        ModifyVelocity((velocity) => VectorUtils.OnAxis(axis, velocity, modification));
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification)
    {
        ModifyVelocity((velocity) => VectorUtils.OnMagnitude(velocity, modification));
    }
    
    public void ModifyVelocityMagnitude(Func<float, float> modification, Vector2 defaultValue)
    {
        ModifyVelocity((velocity) => velocity.sqrMagnitude < 0.001f ? defaultValue : VectorUtils.OnMagnitude(velocity, modification));
    }

    public void FlipAxis(Vector2 axis) =>
        ModifyVelocityOnAxis(axis, a => -a);

    
    public void RotateVelocity(Vector2 newUp)
    {
        RotateVelocity(Up, newUp);
    }

    public void RotateVelocity(Vector2 from, Vector2 to)
    {
        ModifyVelocity((velocity) => Vector2.Dot(velocity, from) * to + 
                                     Vector2.Dot(velocity, new Vector2(from.y, -from.x)) * new Vector2(to.y, -to.x));
    }

    public void FixedUpdate()
    {
        var lastFloorNormal = FloorNormal;
        var lastLeftWallNormal = LeftWallNormal;
        var lastRightWallNormal = RightWallNormal;
        var lastCeilingNormal = CeilingNormal;

        GetContactsFromBody();
        CalculateNormals();
        
        if (!skipSnapping && lastFloorNormal.sqrMagnitude >= ErrorWindow && FloorNormal.sqrMagnitude < ErrorWindow)
            Snap();

        skipSnapping = false;
        
        CheckAndCallEvent(lastFloorNormal, FloorNormal, onFloorEnter, onFloorExit);
        CheckAndCallEvent(lastLeftWallNormal, LeftWallNormal, onLeftWallEnter, onLeftWallExit);
        CheckAndCallEvent(lastRightWallNormal, RightWallNormal, onRightWallEnter, onRightWallExit);
        CheckAndCallEvent(lastCeilingNormal, CeilingNormal, onCeilingEnter, onCeilingExit);
        
        if (IsOnFloor() && IsOnLeftWall())
            LimitSpeed(GetFloorLeft(), 0f);
        
        if (IsOnFloor() && IsOnRightWall())
            LimitSpeed(GetFloorRight(), 0f);
    }
    
    private void GetContactsFromBody()
    {
        ClearCollisions();
        
        var contacts = new List<ContactPoint2D>();
        body.GetContacts(contacts);

        foreach (var normal in contacts.ConvertAll(contact => contact.normal).Where(IsSurfaceStable))
            AddCollision(normal);
    }
    
    private void ClearCollisions()
    {
        Floors.Clear();
        LeftWalls.Clear();
        RightWalls.Clear();
        Ceilings.Clear();
    }
    
    private bool IsSurfaceStable(Vector2 normal) =>
        Vector2.Dot(body.velocity.normalized, normal) < ErrorWindow;

    public void AddCollision(Vector2 point)
    {
        var type = GetContactType(point);
            
        switch (type)
        {
            case CollisionType.Floor:
                Floors.Add(point);
                break;

            case CollisionType.LeftWall:
                LeftWalls.Add(point);
                break;
                
            case CollisionType.RightWall:
                RightWalls.Add(point);
                break;

            case CollisionType.Ceiling:
                Ceilings.Add(point);
                break;
        }
    }

    private CollisionType GetContactType(Vector2 normal)
    {
        var maxFloorCosine = Mathf.Cos(maxFloorAngle * Mathf.Deg2Rad);

        if (Vector2.Dot(up, normal) > maxFloorCosine)
            return CollisionType.Floor;

        if (Vector2.Dot(-up, normal) > maxFloorCosine)
            return CollisionType.Ceiling;

        return IsNormalLeft(normal) ? CollisionType.LeftWall : CollisionType.RightWall;
    }

    private void CheckAndCallEvent(Vector2 lastNormal, Vector2 currentNormal, UnityEvent<Vector2> enter, UnityEvent<Vector2> exit)
    {
        if (lastNormal.sqrMagnitude < ErrorWindow && currentNormal.sqrMagnitude >= ErrorWindow) enter.Invoke(currentNormal);
        else if (lastNormal.sqrMagnitude >= ErrorWindow && currentNormal.sqrMagnitude < ErrorWindow)
            exit.Invoke(lastNormal);
    }

    private void Snap()
    {
        var hits = Cast(Down, maxSnapLength);
        if (hits.Count == 0)
            return;
        
        var hit = hits[0];
        if (GetContactType(hit.normal) != CollisionType.Floor)
            return;
        
        body.position += hit.distance * Down;
        
        var direction = Mathf.Sign(hit.normal.x * Velocity.y - hit.normal.y * Velocity.x);
        ModifyVelocity(velocity => direction * VectorUtils.Orthogonal(hit.normal) * Velocity.magnitude);
        FloorNormal = hit.normal;
        SetSpeed(-FloorNormal, 10f);
    }

    public List<RaycastHit2D> Cast(Vector2 direction, float distance)
    {
        var hits = new List<RaycastHit2D>();
        body.Cast(direction, hits, distance);
        return hits;
    }

    private void CalculateNormals()
    {
        FloorNormal = AvarageNormal(Floors);
        LeftWallNormal = AvarageNormal(LeftWalls);
        RightWallNormal = AvarageNormal(RightWalls);
        CeilingNormal = AvarageNormal(Ceilings);
    }

    private Vector2 AvarageNormal(List<Vector2> vectors) => VectorUtils.Avarage(vectors).normalized;
    
    public Vector2 GetLeftWallUp() => VectorUtils.Orthogonal(LeftWallNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetLeftWallDown() => -GetLeftWallUp();
    
    public Vector2 GetRightWallUp() => -VectorUtils.Orthogonal(RightWallNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetRightWallDown() => -GetRightWallUp();

    public Vector2 GetFloorRight() => -VectorUtils.Orthogonal(FloorNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetFloorLeft() => -GetFloorRight();

    public Vector2 GetCeilingRight() => VectorUtils.Orthogonal(CeilingNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetCeilingLeft() => -GetCeilingRight();

    public bool IsOnFloor() => FloorNormal.sqrMagnitude >= ErrorWindow;
    public bool IsOnLeftWall() => LeftWallNormal.sqrMagnitude >= ErrorWindow;
    public bool IsOnRightWall() => RightWallNormal.sqrMagnitude >= ErrorWindow;
    public bool IsOnCeiling() => CeilingNormal.sqrMagnitude >= ErrorWindow;
    
    public bool IsOnFloorOnly() => IsOnFloor() && !IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    public bool IsOnCeilingOnly() => !IsOnFloor() && !IsOnLeftWall() && !IsOnRightWall() && IsOnCeiling();
    public bool IsOnLeftWallOnly() => !IsOnFloor() && IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    public bool IsOnRightWallOnly() => !IsOnFloor() && !IsOnLeftWall() && IsOnRightWall() && !IsOnCeiling();

    public bool IsLeavingFloor() => IsOnFloor() && Vector2.Dot(FloorNormal, Velocity) > 0.001f;
    public bool IsLeavingLeftWall() => IsOnLeftWall() && Vector2.Dot(LeftWallNormal, Velocity) > 0.001f;
    public bool IsLeavingRightWall() => IsOnRightWall() && Vector2.Dot(RightWallNormal, Velocity) > 0.001f;
    public bool IsLeavingCeiling() => IsOnCeiling() && Vector2.Dot(CeilingNormal, Velocity) > 0.001f;

    public bool IsGoingUp() => Vector2.Dot(Velocity, up) > 0.001f;
    public bool IsGoingDown() => Vector2.Dot(Velocity, Down) > 0.001f;
    public bool HasNoVerticalSpeed() => Mathf.Approximately(0f, Vector2.Dot(Velocity, up));
    public bool IsGoingLeft() => Vector2.Dot(Velocity, Left) > 0.001f;
    public bool IsGoingRight() => Vector2.Dot(Velocity, Right) > 0.001f;
    public bool HasNoHorizontalSpeed() => Mathf.Approximately(0f, Vector2.Dot(Velocity, Left));

    public Vector2 Velocity => body.velocity;
    
    public float GetSpeedUp() => GetSpeedOnAxis(Up);
    public float GetSpeedDown() => GetSpeedOnAxis(Down);
    public float GetSpeedLeft() => GetSpeedOnAxis(Left);
    public float GetSpeedRight() => GetSpeedOnAxis(Right);
    
    public float GetSpeedOnFloorNormal() => GetSpeedOnAxis(FloorNormal);
    public float GetSpeedOnLeftWallNormal() => GetSpeedOnAxis(LeftWallNormal);
    public float GetSpeedOnRightWallNormal() => GetSpeedOnAxis(RightWallNormal);
    public float GetSpeedOnCeilingNormal() => GetSpeedOnAxis(CeilingNormal);
    public float GetSpeedOnFloorRight() => GetSpeedOnAxis(GetFloorRight());
    public float GetSpeedOnFloorLeft() => GetSpeedOnAxis(GetFloorLeft());
    
    public float GetSpeedOnCeilingRight() => GetSpeedOnAxis(GetCeilingRight());
    public float GetSpeedOnCeilingLeft() => GetSpeedOnAxis(GetCeilingLeft());
    public float GetSpeedOnLeftWallUp() => GetSpeedOnAxis(GetLeftWallUp());
    public float GetSpeedOnLeftWallDown() => GetSpeedOnAxis(GetLeftWallDown());
    public float GetSpeedOnRightWallUp() => GetSpeedOnAxis(GetRightWallUp());
    public float GetSpeedOnRightWallDown() => GetSpeedOnAxis(GetRightWallDown());
    
    public float GetSpeedOnAxis(Vector2 axis) => Vector2.Dot(axis, Velocity);

    private bool IsNormalLeft(Vector2 normal) => Vector2.Dot(normal, Right) > 0.001f;
    private bool IsNormalRight(Vector2 normal) => Vector2.Dot(normal, Left) > 0.001f;
}