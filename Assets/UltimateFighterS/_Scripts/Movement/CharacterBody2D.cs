using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

internal enum CollisionType
{
    Floor,
    LeftWall,
    RightWall,
    Ceiling
}

// TODO: Passar para o CharacterBody2D a Logica de Virar o Corpo
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBody2D : MonoBehaviour
{
    private const float ERROR_WINDOW = 0.05f;

    // Up direction and maxAngles define what is considered a floor, a left or right wall or a ceiling
    // A left direction is defined as being counterclockwise from up. Similarly, right when clockwise.
    // In case Up is Vector2.zero, all collisions will be recognized as Left Walls
    [FormerlySerializedAs("up")] [SerializeField] private Vector2 _up = Vector2.up;

    [FormerlySerializedAs("maxFloorAngle")] [SerializeField] [Range(0, 90)] private float _maxFloorAngle = 45;
    [FormerlySerializedAs("maxSnapLength")] [SerializeField] private float _maxSnapLength = 0.5f;

    [FormerlySerializedAs("floors")] public List<Vector2> Floors = new();
    [FormerlySerializedAs("leftWalls")] public List<Vector2> LeftWalls = new();
    [FormerlySerializedAs("rightWalls")] public List<Vector2> RightWalls = new();
    [FormerlySerializedAs("ceilings")] public List<Vector2> Ceilings = new();

    // Events for Collision
    // Enter events are only called if previously there were no correspondent colliders
    // A new collider entering will not call Enter Events if there was already another collider
    [FormerlySerializedAs("onFloorEnter")] [HideInInspector] public UnityEvent<Vector2> OnFloorEnter;
    [FormerlySerializedAs("onLeftWallEnter")] [HideInInspector] public UnityEvent<Vector2> OnLeftWallEnter;
    [FormerlySerializedAs("onRightWallEnter")] [HideInInspector] public UnityEvent<Vector2> OnRightWallEnter;
    [FormerlySerializedAs("onCeilingEnter")] [HideInInspector] public UnityEvent<Vector2> OnCeilingEnter;

    // Exit events are only called if currently there are no correspondent colliders
    // A collider exiting will not call Exit Events if there still one or more colliders remaining
    [FormerlySerializedAs("onFloorExit")] [HideInInspector] public UnityEvent<Vector2> OnFloorExit;
    [FormerlySerializedAs("onLeftWallExit")] [HideInInspector] public UnityEvent<Vector2> OnLeftWallExit;
    [FormerlySerializedAs("onRightWallExit")] [HideInInspector] public UnityEvent<Vector2> OnRightWallExit;
    [FormerlySerializedAs("onCeilingExit")] [HideInInspector] public UnityEvent<Vector2> OnCeilingExit;

    // RigidBody2D used by movement functions.
    private readonly List<Collider2D> _colliders = new();
    private Rigidbody2D _body;
    private bool _skipSnapping;

    public Vector2 Up
    {
        set
        {
            Vector2 newUp = value.normalized;
            RotateVelocity(newUp);
            _up = newUp;
        }
        get => _up;
    }

    public Vector2 Left => new(-_up.y, _up.x);
    public Vector2 Right => new(_up.y, -_up.x);
    public Vector2 Down => -_up;

    // Normals calculated from lists above.
    // A zero vector means there is no contact points.
    public Vector2 FloorNormal { get; private set; } = Vector2.zero;
    public Vector2 LeftWallNormal { get; private set; } = Vector2.zero;
    public Vector2 RightWallNormal { get; private set; } = Vector2.zero;
    public Vector2 CeilingNormal { get; private set; } = Vector2.zero;

    public Vector2 LastVelocity { get; private set; }

    public Vector2 Velocity => _body.velocity;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.GetAttachedColliders(_colliders);
        _body.sleepMode = RigidbodySleepMode2D.NeverSleep;
        _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void FixedUpdate()
    {
        Vector2 lastFloorNormal = FloorNormal;
        Vector2 lastLeftWallNormal = LeftWallNormal;
        Vector2 lastRightWallNormal = RightWallNormal;
        Vector2 lastCeilingNormal = CeilingNormal;

        GetContactsFromBody();
        CalculateNormals();

        if (!_skipSnapping && lastFloorNormal.sqrMagnitude >= ERROR_WINDOW && FloorNormal.sqrMagnitude < ERROR_WINDOW)
            Snap();

        _skipSnapping = false;

        CheckAndCallEvent(lastFloorNormal, FloorNormal, OnFloorEnter, OnFloorExit);
        CheckAndCallEvent(lastLeftWallNormal, LeftWallNormal, OnLeftWallEnter, OnLeftWallExit);
        CheckAndCallEvent(lastRightWallNormal, RightWallNormal, OnRightWallEnter, OnRightWallExit);
        CheckAndCallEvent(lastCeilingNormal, CeilingNormal, OnCeilingEnter, OnCeilingExit);

        /*
        if (IsOnFloor() && IsOnLeftWall())
            SetSpeed(GetFloorLeft(), 0f);

        if (IsOnFloor() && IsOnRightWall())
            SetSpeed(GetFloorRight(), 0f);
        */

        LastVelocity = Velocity;
    }

    public void MoveAndClamp(Vector2 axis, float direction, float acceleration, float turnAcceleration,
        float deceleration,
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

    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration,
        float deceleration,
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

    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration,
        float deceleration,
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

    public void MoveSmoothly(Vector2 axis, float direction, float acceleration, float turnAcceleration,
        float deceleration,
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
            x => x >= maxSpeed ? x : Mathf.MoveTowards(x, maxSpeed, acceleration * Time.fixedDeltaTime)
        );
    }

    public void AccelerateUntil(float acceleration, float maxSpeed)
    {
        ModifyVelocityMagnitude(
            magnitude => magnitude > maxSpeed
                ? magnitude
                : Mathf.MoveTowards(magnitude, maxSpeed, magnitude * Time.fixedDeltaTime)
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
            x => Mathf.Abs(x) <= minSpeed
                ? x
                : Mathf.Sign(x) * Mathf.MoveTowards(Mathf.Abs(x), minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }

    public void DecelerateUntil(float deceleration, float minSpeed)
    {
        ModifyVelocityMagnitude(
            magnitude => magnitude < minSpeed
                ? magnitude
                : Mathf.MoveTowards(magnitude, minSpeed, deceleration * Time.fixedDeltaTime)
        );
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        ModifyVelocity(velocity => velocity + impulse);
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
        ModifyVelocity(velocity => VectorUtils.OnAxis(axis, velocity,
            x => speed
        ));
    }

    public void SetVelocity(Vector2 speed)
    {
        _body.velocity = speed;
        UpdateCurrentContacts();
        _skipSnapping = !IsSurfaceStable(FloorNormal);
    }

    public void ModifyVelocity(Func<Vector2, Vector2> modification)
    {
        SetVelocity(modification(_body.velocity));
    }

    public void ModifyVelocity(Func<float, float> modificationX, Func<float, float> modificationY)
    {
        ModifyVelocity(velocity => new Vector2(modificationX(velocity.x), modificationY(velocity.y)));
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
        ModifyVelocity(velocity => VectorUtils.OnAxis(axis, velocity, modification));
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification)
    {
        ModifyVelocity(velocity => VectorUtils.OnMagnitude(velocity, modification));
    }

    public void ModifyVelocityMagnitude(Func<float, float> modification, Vector2 defaultValue)
    {
        ModifyVelocity(velocity =>
            velocity.sqrMagnitude < 0.001f ? defaultValue : VectorUtils.OnMagnitude(velocity, modification));
    }

    public void FlipAxis(Vector2 axis)
    {
        ModifyVelocityOnAxis(axis, a => -a);
    }


    public void RotateVelocity(Vector2 newUp)
    {
        RotateVelocity(Up, newUp);
    }

    public void RotateVelocity(Vector2 from, Vector2 to)
    {
        ModifyVelocity(velocity => Vector2.Dot(velocity, from) * to +
                                   Vector2.Dot(velocity, new Vector2(from.y, -from.x)) * new Vector2(to.y, -to.x));
    }

    public void RotateVelocity(float angle)
    {
        ModifyVelocity(velocity => VectorUtils.Rotated(velocity, angle));
    }

    public void RotateVelocity(Vector2 towards, float angle)
    {
        ModifyVelocity(delegate(Vector2 velocity)
        {
            float direction = Mathf.Sign(VectorUtils.Wedge(Velocity, towards));
            return VectorUtils.Rotated(velocity, direction * angle);
        });
    }

    public void RotateVelocityUntil(Vector2 target, float angleDelta)
    {
        ModifyVelocity(delegate(Vector2 velocity)
        {
            float angle = Mathf.Acos(Vector2.Dot(target.normalized, velocity.normalized));
            float direction = Mathf.Sign(VectorUtils.Wedge(Velocity, target));
            return VectorUtils.Rotated(velocity, direction * angle);
        });
    }

    public void UpdateCurrentContacts()
    {
        Floors = Floors.Where(IsSurfaceStable).ToList();
        LeftWalls = LeftWalls.Where(IsSurfaceStable).ToList();
        RightWalls = RightWalls.Where(IsSurfaceStable).ToList();
        Ceilings = Ceilings.Where(IsSurfaceStable).ToList();
        CalculateNormals();
    }

    private void GetContactsFromBody()
    {
        ClearCollisions();

        List<ContactPoint2D> contacts = new();
        _body.GetContacts(contacts);

        foreach (Vector2 normal in contacts.ConvertAll(contact => contact.normal).Where(IsSurfaceStable))
            AddCollision(normal);
    }

    private void ClearCollisions()
    {
        Floors.Clear();
        LeftWalls.Clear();
        RightWalls.Clear();
        Ceilings.Clear();
    }

    private bool IsSurfaceStable(Vector2 normal)
    {
        return Vector2.Dot(_body.velocity.normalized, normal) < ERROR_WINDOW;
    }

    public void AddCollision(Vector2 point)
    {
        CollisionType type = GetContactType(point);

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
        float maxFloorCosine = Mathf.Cos(_maxFloorAngle * Mathf.Deg2Rad);

        if (Vector2.Dot(_up, normal) > maxFloorCosine)
            return CollisionType.Floor;

        if (Vector2.Dot(-_up, normal) > maxFloorCosine)
            return CollisionType.Ceiling;

        return IsNormalLeft(normal) ? CollisionType.LeftWall : CollisionType.RightWall;
    }

    private void CheckAndCallEvent(Vector2 lastNormal, Vector2 currentNormal, UnityEvent<Vector2> enter,
        UnityEvent<Vector2> exit)
    {
        if (lastNormal.sqrMagnitude < ERROR_WINDOW && currentNormal.sqrMagnitude >= ERROR_WINDOW)
            enter.Invoke(currentNormal);
        else if (lastNormal.sqrMagnitude >= ERROR_WINDOW && currentNormal.sqrMagnitude < ERROR_WINDOW)
            exit.Invoke(lastNormal);
    }

    public void Snap()
    {
        List<RaycastHit2D> hits = Cast(Down, _maxSnapLength);
        if (hits.Count == 0)
            return;

        RaycastHit2D hit = hits[0];
        if (GetContactType(hit.normal) != CollisionType.Floor)
            return;

        _body.position += hit.distance * Down;

        float direction = Mathf.Sign(hit.normal.x * Velocity.y - hit.normal.y * Velocity.x);
        ModifyVelocity(velocity => direction * VectorUtils.Orthogonal(hit.normal) * Velocity.magnitude);
        FloorNormal = hit.normal;
        SetSpeed(-FloorNormal, 10f);
    }

    public List<RaycastHit2D> Cast(Vector2 direction, float distance)
    {
        List<RaycastHit2D> hits = new();
        _body.Cast(direction, hits, distance);

        return hits
            .Where(hit => _body.IsTouchingLayers(hit.collider.gameObject.layer))
            .Where(hit => hit.normal.sqrMagnitude > 0.01f && hit.distance > 0.01f)
            .ToList();
    }

    private void CalculateNormals()
    {
        FloorNormal = AvarageNormal(Floors);
        LeftWallNormal = AvarageNormal(LeftWalls);
        RightWallNormal = AvarageNormal(RightWalls);
        CeilingNormal = AvarageNormal(Ceilings);
    }

    public void SkipSnappingFrame()
    {
        _skipSnapping = true;
    }

    public void Pause()
    {
        _body.simulated = false;
    }

    public void Resume()
    {
        _body.simulated = true;
    }

    private Vector2 AvarageNormal(List<Vector2> vectors)
    {
        return VectorUtils.Average(vectors).normalized;
    }

    public Vector2 GetLeftWallUp()
    {
        return VectorUtils.Orthogonal(LeftWallNormal) * Mathf.Sign(Vector2.Dot(_up, Vector2.up));
    }

    public Vector2 GetLeftWallDown()
    {
        return -GetLeftWallUp();
    }

    public Vector2 GetRightWallUp()
    {
        return -VectorUtils.Orthogonal(RightWallNormal) * Mathf.Sign(Vector2.Dot(_up, Vector2.up));
    }

    public Vector2 GetRightWallDown()
    {
        return -GetRightWallUp();
    }

    public Vector2 GetFloorRight()
    {
        return -VectorUtils.Orthogonal(FloorNormal) * Mathf.Sign(Vector2.Dot(_up, Vector2.up));
    }

    public Vector2 GetFloorLeft()
    {
        return -GetFloorRight();
    }

    public Vector2 GetCeilingRight()
    {
        return VectorUtils.Orthogonal(CeilingNormal) * Mathf.Sign(Vector2.Dot(_up, Vector2.up));
    }

    public Vector2 GetCeilingLeft()
    {
        return -GetCeilingRight();
    }

    public bool IsOnFloor()
    {
        return FloorNormal.sqrMagnitude >= ERROR_WINDOW;
    }

    public bool IsOnLeftWall()
    {
        return LeftWallNormal.sqrMagnitude >= ERROR_WINDOW;
    }

    public bool IsOnRightWall()
    {
        return RightWallNormal.sqrMagnitude >= ERROR_WINDOW;
    }

    public bool IsOnCeiling()
    {
        return CeilingNormal.sqrMagnitude >= ERROR_WINDOW;
    }

    public bool IsOnFloorOnly()
    {
        return IsOnFloor() && !IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    }

    public bool IsOnCeilingOnly()
    {
        return !IsOnFloor() && !IsOnLeftWall() && !IsOnRightWall() && IsOnCeiling();
    }

    public bool IsOnLeftWallOnly()
    {
        return !IsOnFloor() && IsOnLeftWall() && !IsOnRightWall() && !IsOnCeiling();
    }

    public bool IsOnRightWallOnly()
    {
        return !IsOnFloor() && !IsOnLeftWall() && IsOnRightWall() && !IsOnCeiling();
    }

    public bool IsLeavingFloor()
    {
        return IsOnFloor() && Vector2.Dot(FloorNormal, Velocity) > 0.001f;
    }

    public bool IsLeavingLeftWall()
    {
        return IsOnLeftWall() && Vector2.Dot(LeftWallNormal, Velocity) > 0.001f;
    }

    public bool IsLeavingRightWall()
    {
        return IsOnRightWall() && Vector2.Dot(RightWallNormal, Velocity) > 0.001f;
    }

    public bool IsLeavingCeiling()
    {
        return IsOnCeiling() && Vector2.Dot(CeilingNormal, Velocity) > 0.001f;
    }

    public bool IsGoingUp()
    {
        return Vector2.Dot(Velocity, _up) > 0.001f;
    }

    public bool IsGoingDown()
    {
        return Vector2.Dot(Velocity, Down) > 0.001f;
    }

    public bool HasNoVerticalSpeed()
    {
        return Mathf.Approximately(0f, Vector2.Dot(Velocity, _up));
    }

    public bool IsGoingLeft()
    {
        return Vector2.Dot(Velocity, Left) > 0.001f;
    }

    public bool IsGoingRight()
    {
        return Vector2.Dot(Velocity, Right) > 0.001f;
    }

    public bool HasNoHorizontalSpeed()
    {
        return Mathf.Approximately(0f, Vector2.Dot(Velocity, Left));
    }

    public float GetSpeedUp()
    {
        return GetSpeedOnAxis(Up);
    }

    public float GetSpeedDown()
    {
        return GetSpeedOnAxis(Down);
    }

    public float GetSpeedLeft()
    {
        return GetSpeedOnAxis(Left);
    }

    public float GetSpeedRight()
    {
        return GetSpeedOnAxis(Right);
    }

    public float GetSpeedOnFloorNormal()
    {
        return GetSpeedOnAxis(FloorNormal);
    }

    public float GetSpeedOnLeftWallNormal()
    {
        return GetSpeedOnAxis(LeftWallNormal);
    }

    public float GetSpeedOnRightWallNormal()
    {
        return GetSpeedOnAxis(RightWallNormal);
    }

    public float GetSpeedOnCeilingNormal()
    {
        return GetSpeedOnAxis(CeilingNormal);
    }

    public float GetSpeedOnFloorRight()
    {
        return GetSpeedOnAxis(GetFloorRight());
    }

    public float GetSpeedOnFloorLeft()
    {
        return GetSpeedOnAxis(GetFloorLeft());
    }

    public float GetSpeedOnCeilingRight()
    {
        return GetSpeedOnAxis(GetCeilingRight());
    }

    public float GetSpeedOnCeilingLeft()
    {
        return GetSpeedOnAxis(GetCeilingLeft());
    }

    public float GetSpeedOnLeftWallUp()
    {
        return GetSpeedOnAxis(GetLeftWallUp());
    }

    public float GetSpeedOnLeftWallDown()
    {
        return GetSpeedOnAxis(GetLeftWallDown());
    }

    public float GetSpeedOnRightWallUp()
    {
        return GetSpeedOnAxis(GetRightWallUp());
    }

    public float GetSpeedOnRightWallDown()
    {
        return GetSpeedOnAxis(GetRightWallDown());
    }

    public float GetSpeedOnAxis(Vector2 axis)
    {
        return Vector2.Dot(axis, Velocity);
    }

    private bool IsNormalLeft(Vector2 normal)
    {
        return Vector2.Dot(normal, Right) > 0.001f;
    }

    private bool IsNormalRight(Vector2 normal)
    {
        return Vector2.Dot(normal, Left) > 0.001f;
    }
}