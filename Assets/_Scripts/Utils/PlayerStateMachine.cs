using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerState>
{
    [SerializeField] private CharacterBody2D body;
    [SerializeField] private PlayerInputSystem input;

    private override void Start() {
        body.onCeilingEnter.AddListener(OnCeilingEnter);
        body.onCeilingExit.AddListener(OnCeilingExit);
        body.onFloorEnter.AddListener(OnFloorEnter);
        body.onFloorExit.AddListener(OnFloorExit);
        body.onLeftWallEnter.AddListener(onLeftWallEnter);
        body.onLeftWallExit.AddListener(onLeftWallExit);
        body.onRightWallEnter.AddListener(onRightWallEnter);
        body.onRightWallExit.AddListener(onRightWallExit);
        body.onWallEnter.AddListener(onWallEnter);
        body.onWallExit.AddListener(onWallExit);

        
    }

    protected override void OnTransition()
    {
        Current.body = body;
        Current.input = input;
        Current.stateMachine = this;
    }

    private void Update()
    {
        Current.Process();
    }

    private void FixedUpdate()
    {
        Current.PhysicsProcess();
    }

    private void OnCeilingEnter() {Current.OnCeilingEnter()}
    private void OnCeilingExit() {Current.OnCeilingExit()}

    private void OnFloorEnter() {Current.OnFloorEnter()}
    private void OnFloorExit() {Current.OnFloorExit()}

    private void OnLeftWallEnter() {Current.OnLeftWallEnter()}
    private void OnLeftWallExit() {Current.OnLeftWallExit()}

    private void OnRightEnter() {Current.OnRightEnter()}
    private void OnRightExit() {Current.OnRightExit()}

    private void OnWallEnter() {Current.OnWallEnter()}
    private void OnWallExit() {Current.OnWallExit()}
}