using System;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class TimedTransitionBehaviour : CharacterState
{
    [SerializeField] private CharacterState next;
    private Timer timer;

    private void Awake()
    {
        timer = GetComponent<Timer>();
    }

    public override void Enter()
    {
        timer.onTimerFinish.AddListener(OnTimerFinish);
        timer.Init();
    }

    public override void Exit()
    {
        timer.onTimerFinish.RemoveListener(OnTimerFinish);
    }

    private void OnTimerFinish()
    {
        machine.TransitionTo(next);
    }
}