using UnityEngine;

[RequireComponent(typeof(CharacterBody2D))]
public class CharacterStateMachine : StateMachine<CharacterState>
{
    private CharacterBody2D body;
    private InputSystem input;
    private InputBuffer inputBuffer;
    public Transform flipPivotPoint;

    private void Awake()
    {
        body = GetComponent<CharacterBody2D>();
        input = GetComponent<InputSystem>();
        inputBuffer = GetComponent<InputBuffer>();
    }
    
    protected override void OnStart() {
        body.onCeilingEnter.AddListener(OnCeilingEnter);
        body.onCeilingExit.AddListener(OnCeilingExit);
        body.onFloorEnter.AddListener(OnFloorEnter);
        body.onFloorExit.AddListener(OnFloorExit);
        body.onLeftWallEnter.AddListener(OnLeftWallEnter);
        body.onLeftWallExit.AddListener(OnLeftWallExit);
        body.onRightWallEnter.AddListener(OnRightWallEnter);
        body.onRightWallExit.AddListener(OnRightWallExit);
    }

    protected override void OnTransition()
    {
        if (Current is null)
            return;
        
        Current.machine = this;
        Current.body = body;
        Current.input = input;
        Current.inputBuffer = inputBuffer;
        Current.FlipPivotPoint = flipPivotPoint;
    }

    private void Update()
    {
        current?.Process();
    }

    private void FixedUpdate()
    {
        current?.PhysicsProcess();
    }

    private void OnCeilingEnter(Vector2 normal) {Current?.OnCeilingEnter(normal);}
    private void OnCeilingExit(Vector2 normal) {Current?.OnCeilingExit(normal);}

    private void OnFloorEnter(Vector2 normal) {Current?.OnFloorEnter(normal);}
    private void OnFloorExit(Vector2 normal) {Current?.OnFloorExit(normal);}

    private void OnLeftWallEnter(Vector2 normal) {Current?.OnLeftWallEnter(normal);}
    private void OnLeftWallExit(Vector2 normal) {Current?.OnLeftWallExit(normal);}

    private void OnRightWallEnter(Vector2 normal) {Current?.OnRightWallEnter(normal);}
    private void OnRightWallExit(Vector2 normal) {Current?.OnRightWallExit(normal);}
}