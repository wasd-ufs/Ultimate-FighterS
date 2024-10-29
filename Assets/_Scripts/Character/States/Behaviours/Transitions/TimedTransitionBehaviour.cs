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
    }

    public override void Enter()
    {
        timer.Init();
        skip = false;
    }

    public override void Exit()
    {
        if (!timer.IsFinished())
            skip = true;
    }

    private void OnTimerFinish()
    {
        if (skip)
            return;
        
        machine.TransitionTo(next);
    }
}