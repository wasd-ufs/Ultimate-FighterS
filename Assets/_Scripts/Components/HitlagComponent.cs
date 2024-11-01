using UnityEngine;
using UnityEngine.Events;

public class HitlagComponent : CharacterState
{
    [Header ("References")] 
    [SerializeField] private HitlagState hitlagstate;
    [SerializeField] private StateMachine<CharacterState> stateMachine;
    public UnityEvent<float> onHitlag;
    
    public void Apply(float durationHitlag, bool withoutStateChange = false)
    {
        if (hitlagstate != null && stateMachine != null) {
            if (withoutStateChange)
            {
                lag.waitTime = durationHitlag;
                lag.Init();
                onHitlag.Invoke(durationHitlag);
                return;
            }
            
            hitlagstate.SetHitlagTime(durationHitlag);
            stateMachine.TransitionTo(hitlagstate);
            onHitlag.Invoke(durationHitlag);
        }
    }
}
