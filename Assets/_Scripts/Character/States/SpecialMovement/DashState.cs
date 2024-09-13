using System;
using UnityEngine;

public class DashState : CharacterState
{
    [SerializeField] private float speed;
    [SerializeField] private Timer timer;

    private float finalSpeed;

    public override void Enter()
    {
        finalSpeed = body.GetSpeedRight();
        
        body.SetVelocity(speed * body.Right * transform.localScale.x);
        timer.Init();
    }

    public override void Exit()
    {
        body.SetVelocity(finalSpeed * body.Right);
    }
}