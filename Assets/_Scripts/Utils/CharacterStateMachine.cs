using UnityEngine;

public class CharacterStateMachine : StateMachine<CharacterState>
{
    [SerializeField] private CharacterBody2D body;
    [SerializeField] private InputSystem input;

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
        Current.body = body;
        Current.input = input;
        Current.machine = this;
    }

    private void Update()
    {
        Current.Process();
    }

    private void FixedUpdate()
    {
        Current.PhysicsProcess();
    }

    private void OnCeilingEnter() {Current.OnCeilingEnter();}
    private void OnCeilingExit() {Current.OnCeilingExit();}

    private void OnFloorEnter() {Current.OnFloorEnter();}
    private void OnFloorExit() {Current.OnFloorExit();}

    private void OnLeftWallEnter() {Current.OnLeftWallEnter();}
    private void OnLeftWallExit() {Current.OnLeftWallExit();}

    private void OnRightWallEnter() {Current.OnRightWallEnter();}
    private void OnRightWallExit() {Current.OnRightWallExit();}

    private void OnWallEnter() {Current.OnWallEnter();}
    private void OnWallExit() {Current.OnWallExit();}
}