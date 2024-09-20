using System;
//using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class DecoratedCharacterState : CharacterState
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;

    public UnityEvent<Vector2> onCeilingEnterEvent;
    public UnityEvent<Vector2> onCeilingExitEvent;

    public UnityEvent<Vector2> onFloorEnterEvent;
    public UnityEvent<Vector2> onFloorExitEvent;

    public UnityEvent<Vector2> onLeftWallEnterEvent;
    public UnityEvent<Vector2> onLeftWallExitEvent;

    public UnityEvent<Vector2> onRightWallEnterEvent;
    public UnityEvent<Vector2> onRightWallExitEvent;

    public UnityEvent<Vector2> onWallEnterEvent;
    public UnityEvent<Vector2> onWallExitEvent;
    
    [Header("Who im Decorating")]
    [SerializeField] private CharacterState state;
    
    void Configure()
    {
        state.machine = machine;
        state.input = input;
        state.body = body;
    }
    
    public override void Enter()
    {
        Configure();
        
        onEnter.Invoke();
        state.Enter();
    }
    
    public override void Exit()
    {
        onExit.Invoke();
        state.Exit();
    }

    public override void Process()
    {
        state.Process();
    }

    public override void PhysicsProcess()
    {
        state.PhysicsProcess();
    }

    public override void OnCeilingEnter(Vector2 normal) => Decorate(onCeilingEnterEvent, state.OnCeilingEnter, normal);
    public override void OnCeilingExit(Vector2 normal) => Decorate(onCeilingExitEvent, state.OnCeilingExit, normal);
    public override void OnFloorEnter(Vector2 normal) => Decorate(onFloorEnterEvent, state.OnFloorEnter, normal);
    public override void OnFloorExit(Vector2 normal) => Decorate(onFloorExitEvent, state.OnFloorExit, normal);
    public override void OnLeftWallEnter(Vector2 normal) => Decorate(onLeftWallEnterEvent, state.OnLeftWallEnter, normal);
    public override void OnLeftWallExit(Vector2 normal) => Decorate(onLeftWallExitEvent, state.OnLeftWallExit, normal);
    public override void OnRightWallEnter(Vector2 normal) => Decorate(onRightWallEnterEvent, state.OnRightWallEnter, normal);
    public override void OnRightWallExit(Vector2 normal) => Decorate(onRightWallExitEvent, state.OnRightWallExit, normal);
    public override void OnWallEnter(Vector2 normal) => Decorate(onWallEnterEvent, state.OnWallEnter, normal);
    public override void OnWallExit(Vector2 normal) => Decorate(onWallExitEvent, state.OnWallExit, normal);
    
    private void Decorate(UnityEvent<Vector2> decoration, Action<Vector2> action, Vector2 normal)
    {
        decoration.Invoke(normal);
        action.Invoke(normal);
    }
}