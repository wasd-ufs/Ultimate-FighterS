using System;
using UnityEngine;
using UnityEngine.Events;

public class HitlagComponent : CharacterState
{
    [SerializeField] private CharacterState hitlagstate;
    [HideInInspector] public UnityEvent<float> onHitlag = new();
    
    private Timer hitlagTimer;
    private StateMachine<CharacterState> stateMachine;
    

    public void Start()
    {
        hitlagTimer = hitlagstate.GetComponent<Timer>();
        stateMachine = GetComponent<StateMachine<CharacterState>>();
    }

    public void Apply(float durationHitlag)
    {
        if (hitlagstate != null && stateMachine != null) {
             hitlagTimer.waitTime = durationHitlag;
             hitlagTimer.Init();
             
             stateMachine.TransitionTo(hitlagstate);
             onHitlag.Invoke(durationHitlag);
        }
    }

    public void OnDestroy()
    {
        onHitlag.RemoveAllListeners();
    }
}
