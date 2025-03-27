using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CompositeState : CharacterState
{
    [Header("States outside of this object that should be included. Others Are Automatically included.")]
    [SerializeField] private List<CharacterState> foreignStates = new();

    [SerializeField] private bool automaticallyIncludeChildren = false;

    // All States used by the composite
    private HashSet<CharacterState> states = new();
    private bool skip;

    private void Awake()
    {
        if (GetComponent<CharacterState>() != this)
            Destroy(this);
        
        GrabStates();
    }

    private void GrabStates()
    {
        states.Clear();
        
        var local = GetComponents<CharacterState>().ToList();
        local.Remove(this);
        
        states.AddRange(local);
        states.AddRange(foreignStates);

        if (automaticallyIncludeChildren)
        {
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent(out CharacterState child))
                    states.Add(child);
        }
    }
    
    private void Configure(CharacterState state)
    {
        state.Body = Body;
        state.Input = Input;
        state.Machine = Machine;
        state.InputBuffer = InputBuffer;
        state.FlipPivotPoint = FlipPivotPoint;
        state.DustParticles = DustParticles;
    }

    private void ForEachState(Action<CharacterState> action)
    {
        foreach (var state in states)
        {
            Configure(state);
            action(state);
            
            if (skip)
                break;
        }
    }
    
    public override void Enter()
    {
        skip = false;
        ForEachState(Configure);
        ForEachState(state => state.Enter());
    }

    public override void Exit()
    {
        ForEachState(state => state.Exit());
        skip = true;
    }

    public override void Process()
    {
        ForEachState(state => state.Process());
    }

    public override void PhysicsProcess()
    {
        ForEachState(state => state.PhysicsProcess());
    }

    public override void OnCeilingEnter(Vector2 normal)
    {
        ForEachState(state => state.OnCeilingEnter(normal));
    }

    public override void OnCeilingExit(Vector2 normal)
    {
        ForEachState(state => state.OnCeilingExit(normal));
    }

    public override void OnFloorEnter(Vector2 normal)
    {
        ForEachState(state => state.OnFloorEnter(normal));
    }

    public override void OnFloorExit(Vector2 normal)
    {
        ForEachState(state => state.OnFloorExit(normal));
    }

    public override void OnLeftWallEnter(Vector2 normal)
    {
        ForEachState(state => state.OnLeftWallEnter(normal));
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        ForEachState(state => state.OnLeftWallExit(normal));
    }

    public override void OnRightWallEnter(Vector2 normal)
    {
        ForEachState(state => state.OnRightWallEnter(normal));
    }

    public override void OnRightWallExit(Vector2 normal)
    {
        ForEachState(state => state.OnRightWallExit(normal));
    }
}