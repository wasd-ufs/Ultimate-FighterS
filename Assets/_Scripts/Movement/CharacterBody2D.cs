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
    
    // Categorized lists containing contact information from body
    private readonly List<ContactPoint2D> floors = new();
    private readonly List<ContactPoint2D> leftWalls = new();
    private readonly List<ContactPoint2D> rightWalls = new();
    private readonly List<ContactPoint2D> ceilings = new();
    
    // Normals calculated from lists above.
    // A zero vector means there is no contact points.
    public Vector2 FloorNormal { get; private set; } = Vector2.zero;
    public Vector2 WallNormal { get; private set; } = Vector2.zero;
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
    private Vector2 velocity = Vector2.zero;

    // Buffered snap from last frame
    private SnapCollision2D snap;

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
        velocity += acceleration * Time.fixedDeltaTime;
    }

    public void Accelerate(float acceleration)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
            magnitude => magnitude + acceleration * Time.fixedDeltaTime
        );
    }

    public void AccelerateUntil(Vector2 axis, float acceleration, float maxSpeed)
    {
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => (x >= maxSpeed) ? x : Mathf.MoveTowards(x, maxSpeed, acceleration * Time.fixedDeltaTime)
        );
    }

    public void AccelerateUntil(float acceleration, float maxSpeed)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
            magnitude => (magnitude > maxSpeed) ? magnitude : Mathf.MoveTowards(magnitude, maxSpeed, magnitude * Time.fixedDeltaTime)
        );
    }
    
    public void Decelerate(Vector2 axis, float deceleration)
    {
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => Mathf.MoveTowards(x, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void Decelerate(float deceleration)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
            magnitude => Mathf.MoveTowards(magnitude, 0f, deceleration * Time.fixedDeltaTime)
        );
    }

    public void DecelerateUntil(Vector2 axis, float deceleration, float minSpeed)
    {
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => (Mathf.Abs(x) <= minSpeed) ? x : Mathf.Sign(x) * Mathf.MoveTowards(Mathf.Abs(x), minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }
    
    public void DecelerateUntil(float deceleration, float minSpeed)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
            magnitude => (magnitude < minSpeed) ? magnitude : Mathf.MoveTowards(magnitude, minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        velocity += impulse;
    }

    public void ClampSpeed(Vector2 axis, float minimum, float maximum)
    {
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => Mathf.Clamp(x, minimum, maximum)
        );
    }

    public void ClampSpeed(float minimum, float maximum)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
            magnitude => Mathf.Clamp(magnitude, minimum, maximum)
        );
    }

    public void LimitSpeed(Vector2 axis, float maximum)
    {
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => Mathf.Min(x, maximum)
        );
    }

    public void LimitSpeed(float maximum)
    {
        velocity = VectorUtils.OnMagnitude(velocity,
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
        velocity = VectorUtils.OnAxis(axis, velocity,
            x => speed
        );
    }

    public void SetVelocity(Vector2 speed)
    {
        velocity = speed;
    }
    
    public void ModifyVelocity(Func<Vector2, Vector2> modification)
    {
        velocity = modification(velocity);
    }
    
    public void ModifyVelocity(Func<float, float> modificationX, Func<float, float> modificationY)
    {
        velocity.x = modificationX(velocity.x);
        velocity.y = modificationY(velocity.y);
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
        velocity = VectorUtils.OnAxis(axis, velocity, modification);
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification)
    {
        velocity = VectorUtils.OnMagnitude(velocity, modification);
    }

    public void FlipAxis(Vector2 axis) =>
        ModifyVelocityOnAxis(axis, a => -a);

    public void ModifyVelocityMagnitude(Func<float, float> modification, Vector2 defaultValue)
    {
        if (Mathf.Approximately(velocity.sqrMagnitude, 0f))
            velocity = defaultValue;
        
        else
            ModifyVelocityMagnitude(modification);
    }
    
    public void RotateVelocity(Vector2 newUp)
    {
        velocity = Vector2.Dot(velocity, Up) * newUp + 
                   Vector2.Dot(velocity, new Vector2(Up.y, -Up.x)) * new Vector2(newUp.y, -newUp.x);
    }

    public void RotateVelocity(Vector2 from, Vector2 to)
    {
        velocity = Vector2.Dot(velocity, from) * to + 
                   Vector2.Dot(velocity, new Vector2(from.y, -from.x)) * new Vector2(to.y, -to.x);
    }

    private void FixedUpdate()
    {
        if (snap is not null)
            ApplySnap();
        
        UpdateCollisions();
        body.velocity = velocity;
    }

    private void UpdateCollisions()
    {
        var lastFloorNormal = FloorNormal;
        var lastLeftWallNormal = LeftWallNormal;
        var lastRightWallNormal = RightWallNormal;
        var lastWallNormal = WallNormal;
        var lastCeilingNormal = CeilingNormal;
        
        RecalculateCollisions();
        CalculateNormals();
        
        CheckAndCallEvent(lastFloorNormal, FloorNormal, onFloorEnter, onFloorExit);
        CheckAndCallEvent(lastLeftWallNormal, LeftWallNormal, onLeftWallEnter, onLeftWallExit);
        CheckAndCallEvent(lastRightWallNormal, RightWallNormal, onRightWallEnter, onRightWallExit);
        CheckAndCallEvent(lastWallNormal, WallNormal, onWallEnter, onWallExit);
        CheckAndCallEvent(lastCeilingNormal, CeilingNormal, onCeilingEnter, onCeilingExit);
        
        LimitSpeed(-FloorNormal, 0f);
        
        if (IsOnFloorOnly())
            CheckAndBufferSnap(FloorNormal, Down, maxFloorAngle);

        if (IsOnFloor() && IsOnLeftWall())
            LimitSpeed(GetFloorLeft(), 0f);
        
        if (IsOnFloor() && IsOnRightWall())
            LimitSpeed(GetFloorRight(), 0f);
        
        LimitSpeed(-LeftWallNormal, 0f);
        LimitSpeed(-RightWallNormal, 0f);
        LimitSpeed(-CeilingNormal, 0f);
    }
    
    private void RecalculateCollisions()
    {
        ClearCollisions();
        
        var contacts = new List<ContactPoint2D>();
        body.GetContacts(contacts);

        foreach (var point in contacts.Where(IsSurfaceStable))
            AddCollision(point);
    }
    
    private void ClearCollisions()
    {
        floors.Clear();
        leftWalls.Clear();
        rightWalls.Clear();
        ceilings.Clear();
    }
    
    private bool IsSurfaceStable(ContactPoint2D point) =>
        Vector2.Dot(body.velocity, point.normal) < ErrorWindow;

    public void AddCollision(ContactPoint2D point)
    {
        var type = GetContactType(point);
            
        switch (type)
        {
            case CollisionType.Floor:
                floors.Add(point);
                break;

            case CollisionType.LeftWall:
                leftWalls.Add(point);
                break;
                
            case CollisionType.RightWall:
                rightWalls.Add(point);
                break;

            case CollisionType.Ceiling:
                ceilings.Add(point);
                break;
        }
    }

    private CollisionType GetContactType(ContactPoint2D contact)
    {
        var maxFloorCosine = Mathf.Cos(maxFloorAngle * Mathf.Deg2Rad);

        if (Vector2.Dot(up, contact.normal) > maxFloorCosine)
            return CollisionType.Floor;

        if (Vector2.Dot(-up, contact.normal) > maxFloorCosine)
            return CollisionType.Ceiling;

        return IsNormalLeft(contact.normal) ? CollisionType.LeftWall : CollisionType.RightWall;
    }

    private void CheckAndCallEvent(Vector2 lastNormal, Vector2 currentNormal, UnityEvent<Vector2> enter, UnityEvent<Vector2> exit)
    {
        if (lastNormal.sqrMagnitude < ErrorWindow && currentNormal.sqrMagnitude >= ErrorWindow) enter.Invoke(currentNormal);
        else if (lastNormal.sqrMagnitude >= ErrorWindow && currentNormal.sqrMagnitude < ErrorWindow) exit.Invoke(lastNormal);
    }

    private void CheckAndBufferSnap(Vector2 currentNormal, Vector2 snapDirection, float maxVariation)
    {
        if (currentNormal.sqrMagnitude < ErrorWindow)
            return;
        
        if (Vector2.Dot(body.velocity.normalized, currentNormal) > ErrorWindow)
            return;

        var snapCheckPos = body.position + body.velocity * Time.fixedDeltaTime;
        var hits = Cast(
            snapCheckPos,
            snapDirection,
            maxSnapLength + ErrorWindow
        );

        if (hits.Count == 0 || hits[0].distance < ErrorWindow)
            return;

        var maxVariationCosine = Mathf.Cos(maxVariation * Mathf.Deg2Rad);
        if (Vector2.Dot(hits[0].normal, -snapDirection) <= maxVariationCosine)
            return;
        
        snap = new SnapCollision2D(snapCheckPos + snapDirection * (hits[0].distance + ErrorWindow), currentNormal,
            hits[0].normal);
        
        body.isKinematic = true;
    }

    public List<RaycastHit2D> Cast(Vector2 position, Vector2 direction, float distance)
    {
        var offset = position - body.position;
        foreach (var collider in colliders)
            collider.offset += offset;

        var hits = new List<RaycastHit2D>();
        body.Cast(direction, hits, distance);
        
        foreach (var collider in colliders)
            collider.offset -= offset;

        return hits;
    }

    private void ApplySnap()
    {
        body.position = snap.position;
        RotateVelocity(snap.lastNormal, snap.normal);
        
        Debug.Log(snap.normal);
        
        body.velocity -= snap.normal * ErrorWindow / Time.fixedDeltaTime;
        snap = null;
        body.isKinematic = false;
    }

    private void CalculateNormals()
    {
        FloorNormal = AvarageNormal(GetFloorNormals());
        WallNormal = AvarageNormal(GetWallNormals());
        LeftWallNormal = AvarageNormal(GetLeftWallNormals());
        RightWallNormal = AvarageNormal(GetRightWallNormals());
        CeilingNormal = AvarageNormal(GetCeilingNormals());
    }

    private Vector2 AvarageNormal(List<Vector2> vectors) => VectorUtils.Avarage(vectors).normalized;

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

    public List<ContactPoint2D> GetCollidingPoints()
    {
        var combinedList = new List<ContactPoint2D>(floors);
        combinedList.AddRange(leftWalls);
        combinedList.AddRange(rightWalls);
        combinedList.AddRange(ceilings);

        return combinedList;
    }
    
    public List<Vector2> GetFloorNormals() => GetFloorPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetWallNormals() => GetWallPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetLeftWallNormals() => GetLeftWallPoints().ConvertAll(point => point.normal);    
    public List<Vector2> GetRightWallNormals() => GetRightWallPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetCeilingNormals() => GetCeilingPoints().ConvertAll(point => point.normal);
    public List<Vector2> GetCollidingNormals() => GetCollidingPoints().ConvertAll(point => point.normal);

    public int GetFloorContactCount() => GetFloorPoints().Count;
    public int GetWallContactCount() => GetWallPoints().Count;
    public int GetLeftWallContactCount() => GetLeftWallPoints().Count;
    public int GetRightWallContactCount() => GetRightWallPoints().Count;
    public int GetCeilingContactCount() => GetCeilingPoints().Count;
    
    public Vector2 GetLeftWallUp() => VectorUtils.Orthogonal(LeftWallNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetLeftWallDown() => -GetLeftWallUp();
    
    public Vector2 GetRightWallUp() => -VectorUtils.Orthogonal(RightWallNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetRightWallDown() => -GetRightWallUp();

    public Vector2 GetFloorRight() => -VectorUtils.Orthogonal(FloorNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetFloorLeft() => -GetFloorRight();

    public Vector2 GetCeilingRight() => VectorUtils.Orthogonal(CeilingNormal) * Mathf.Sign(Vector2.Dot(up, Vector2.up));
    public Vector2 GetCeilingLeft() => -GetCeilingRight();

    public bool IsOnFloor() => floors.Count > 0;
    public bool IsOnLeftWall() => leftWalls.Count > 0;
    public bool IsOnRightWall() => rightWalls.Count > 0;
    public bool IsOnWall() => IsOnLeftWall() || IsOnRightWall();
    public bool IsOnCeiling() => ceilings.Count > 0;
    
    public bool IsOnFloorOnly() => IsOnFloor() && !IsOnWall() && !IsOnCeiling();
    public bool IsOnWallOnly() => !IsOnFloor() && IsOnWall() && !IsOnCeiling();
    public bool IsOnCeilingOnly() => !IsOnFloor() && !IsOnWall() && IsOnCeiling();
    public bool IsOnLeftWallOnly() => !IsOnFloor() && IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    public bool IsOnRightWallOnly() => !IsOnFloor() && !IsOnLeftWall() && IsOnRightWall() && !IsOnCeiling();

    public bool IsLeavingFloor() => IsOnFloor() && Vector2.Dot(FloorNormal, GetVelocity()) > 0.001f;
    public bool IsLeavingWall() => IsOnWall() && Vector2.Dot(WallNormal, GetVelocity()) > 0.001f;
    public bool IsLeavingLeftWall() => IsOnLeftWall() && Vector2.Dot(LeftWallNormal, GetVelocity()) > 0.001f;
    public bool IsLeavingRightWall() => IsOnRightWall() && Vector2.Dot(RightWallNormal, GetVelocity()) > 0.001f;
    public bool IsLeavingCeiling() => IsOnCeiling() && Vector2.Dot(CeilingNormal, GetVelocity()) > 0.001f;

    public bool IsGoingUp() => Vector2.Dot(velocity, up) > 0.001f;
    public bool IsGoingDown() => Vector2.Dot(velocity, Down) > 0.001f;
    public bool HasNoVerticalSpeed() => Mathf.Approximately(0f, Vector2.Dot(velocity, up));
    public bool IsGoingLeft() => Vector2.Dot(velocity, Left) > 0.001f;
    public bool IsGoingRight() => Vector2.Dot(velocity, Right) > 0.001f;
    public bool HasNoHorizontalSpeed() => Mathf.Approximately(0f, Vector2.Dot(velocity, Left));

    public Vector2 GetVelocity() => velocity;
    
    public float GetSpeedUp() => GetSpeedOnAxis(Up);
    public float GetSpeedDown() => GetSpeedOnAxis(Down);
    public float GetSpeedLeft() => GetSpeedOnAxis(Left);
    public float GetSpeedRight() => GetSpeedOnAxis(Right);
    public float GetSpeedOnFloorNormal() => GetSpeedOnAxis(FloorNormal);
    public float GetSpeedOnLeftWallNormal() => GetSpeedOnAxis(LeftWallNormal);
    public float GetSpeedOnRightWallNormal() => GetSpeedOnAxis(RightWallNormal);
    public float GetSpeedOnWallNormal() => GetSpeedOnAxis(WallNormal);
    public float GetSpeedOnCeilingNormal() => GetSpeedOnAxis(CeilingNormal);
    public float GetSpeedOnFloorRight() => GetSpeedOnAxis(GetFloorRight());
    public float GetSpeedOnFloorLeft() => GetSpeedOnAxis(GetFloorLeft());
    public float GetSpeedOnLeftWallUp() => GetSpeedOnAxis(GetLeftWallUp());
    public float GetSpeedOnLeftWallDown() => GetSpeedOnAxis(GetLeftWallDown());
    public float GetSpeedOnRightWallUp() => GetSpeedOnAxis(GetRightWallUp());
    public float GetSpeedOnRightWallDown() => GetSpeedOnAxis(GetRightWallDown());
    
    public float GetSpeedOnAxis(Vector2 axis) => Vector2.Dot(axis, velocity);

    private bool IsNormalLeft(Vector2 normal) => Vector2.Dot(normal, Right) > 0.001f;
    private bool IsNormalRight(Vector2 normal) => Vector2.Dot(normal, Left) > 0.001f;
}

class SnapCollision2D
{
    public Vector2 position;
    public Vector2 lastNormal;
    public Vector2 normal;

    public SnapCollision2D(Vector2 pos, Vector2 last, Vector2 nor)
    {
        position = pos;
        lastNormal = last;
        normal = nor;
    }
}