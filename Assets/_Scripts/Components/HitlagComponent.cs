using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class HitlagComponent : CharacterState
{
    [SerializeField] private CharacterState hitlagstate;
    [SerializeField] private CharacterState hitstopstate;
    
    [HideInInspector] public UnityEvent<float> onHitlag = new();
    
    private Timer hitlagTimer;
    private Timer hitstopTimer;
    
    private StateMachine<CharacterState> stateMachine;
    
    public void Start()
    {
        hitlagTimer = hitlagstate.GetComponent<Timer>();
        hitstopTimer = hitstopstate.GetComponent<Timer>();
        
        stateMachine = GetComponent<StateMachine<CharacterState>>();
    }

    public void Apply(float durationHitlag, float durationHitstop)
    {
        if (hitlagstate != null && hitstopTimer != null && stateMachine != null)
        {
            hitlagTimer.waitTime = durationHitlag;
            hitstopTimer.waitTime = durationHitstop;
            
            stateMachine.TransitionTo(hitstopstate);
            onHitlag.Invoke(durationHitlag);
        }
    }

    public void OnDestroy()
    {
        onHitlag.RemoveAllListeners();
    }
}
