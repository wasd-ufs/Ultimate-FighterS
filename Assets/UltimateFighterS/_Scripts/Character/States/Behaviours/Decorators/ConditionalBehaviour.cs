using System;
using UnityEngine;

public class ConditionalBehaviour : CharacterState
{
    [SerializeField] public CharacterState subordinate;
    [SerializeField] public bool run;

    private void TryRun(Action<CharacterState> action)
    {
        if (run)
        {
            Configure(subordinate);
            action.Invoke(subordinate);
        }
    }

    private void Configure(CharacterState state)
    {
        state.machine = machine;
        state.body = body;
        state.input = input;
        state.inputBuffer = inputBuffer;
        state.FlipPivotPoint = FlipPivotPoint;
        state.DustParticles = DustParticles;
    }
    
    public override void Enter()
    {
        TryRun(state => state.Enter());
    }

    public override void Exit()
    {
        TryRun(state => state.Exit());
    }

    public override void Process()
    {
        TryRun(state => state.Process());
    }

    public override void PhysicsProcess()
    {
        TryRun(state => state.PhysicsProcess());
    }

    public override void OnCeilingEnter(Vector2 normal)
    {
        TryRun(state => state.OnCeilingEnter(normal));
    }

    public override void OnCeilingExit(Vector2 normal)
    {
        TryRun(state => state.OnCeilingExit(normal));
    }

    public override void OnFloorEnter(Vector2 normal)
    {
        TryRun(state => state.OnFloorEnter(normal));
    }

    public override void OnFloorExit(Vector2 normal)
    {
        TryRun(state => state.OnFloorExit(normal));
    }

    public override void OnLeftWallEnter(Vector2 normal)
    {
        TryRun(state => state.OnLeftWallEnter(normal));
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        TryRun(state => state.OnLeftWallExit(normal));
    }

    public override void OnRightWallEnter(Vector2 normal)
    {
        TryRun(state => state.OnRightWallEnter(normal));
    }

    public override void OnRightWallExit(Vector2 normal)
    {
        TryRun(state => state.OnRightWallExit(normal));
    }
}