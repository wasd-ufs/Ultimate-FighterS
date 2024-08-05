using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitlagState : CharacterState
{
    [Header("Transitions")]
    [SerializeField] private CharacterState airborne;

    [Header("Timer")]
    private float TimeInHitlag;
    private float timer;
    private bool isFastFalling;
    
    [Header("Vertical")]
    [SerializeField] private float gravityForce;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fastFallGravity;

    void Start()
    {
        timer = 0;
    }

    public override void Process()
    {
       isFastFalling = isFastFalling || !input.IsSpecialBeingHeld();
    }

    public override void PhysicsProcess()
    {
        addtime();

        var gravity = isFastFalling ? fastFallGravity : gravityForce;
        body.Accelerate(body.Down * gravity);
    
        body.LimitSpeed(body.Down, maxFallSpeed);

        if (timer >= TimeInHitlag) machine.TransitionTo(airborne);
    }

    private void addtime()
    {
        timer += Time.fixedDeltaTime;
    }

    public void SetHitlagTime(float time)
    {
        TimeInHitlag = time;
    }
}
