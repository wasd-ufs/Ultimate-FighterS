using UnityEngine;

public class CharacterStateMachine : StateMachine<CharacterState>
{
    [SerializeField] private CharacterBody2D body;
    [SerializeField] private InputSystem input;
    [SerializeField] private FacingDirection facingDirection;
    [SerializeField] private Transform player;


    protected override void OnStart() {
        body.onCeilingEnter.AddListener(OnCeilingEnter);
        body.onCeilingExit.AddListener(OnCeilingExit);
        body.onFloorEnter.AddListener(OnFloorEnter);
        body.onFloorExit.AddListener(OnFloorExit);
        body.onLeftWallEnter.AddListener(OnLeftWallEnter);
        body.onLeftWallExit.AddListener(OnLeftWallExit);
        body.onRightWallEnter.AddListener(OnRightWallEnter);
        body.onRightWallExit.AddListener(OnRightWallExit);
        body.onWallEnter.AddListener(OnWallEnter);
        body.onWallExit.AddListener(OnWallExit);
    }

    protected override void OnTransition()
    {
        if (Current is null)
            return;
        
        Current.body = body;
        Current.input = input;
        Current.machine = this;
        facingDirection.player = player;
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

    private void OnWallEnter(Vector2 normal) {Current?.OnWallEnter(normal);}
    private void OnWallExit(Vector2 normal) {Current?.OnWallExit(normal);}
}