using System;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class TimedTransitionBehaviour : CharacterState
{
    [SerializeField] private CharacterState next;
    private Timer timer;
    private bool skip;

    private void Awake()
    {
        timer = GetComponent<Timer>();
        timer.onTimerFinish.AddListener(OnTimerFinish);
        skip = true;
    }

    public override void Enter()
    {
        skip = false;
        timer.Init();
        
        timer.enabled = true;
    }

    public override void Process()
    {
        timer.Update();
    }

    public override void Exit()
    {
        skip = true;
        timer.enabled = false;
    }

    private void OnTimerFinish()
    {
        if (skip)
            return;
        
        machine.TransitionTo(next);
    }
}