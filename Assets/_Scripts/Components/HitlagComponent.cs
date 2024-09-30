using UnityEngine;
using UnityEngine.Events;

public class HitlagComponent : CharacterState
{
    [Header ("References")] 
    [SerializeField] private CharacterState hitlagstate;
    [SerializeField] private Timer hitlagTimer;
    [SerializeField] private StateMachine<CharacterState> stateMachine;
    public UnityEvent<float> onHitlag;
    
    public void Apply(float durationHitlag)
    {
        if (hitlagstate != null && stateMachine != null) {
             hitlagTimer.waitTime = durationHitlag;
             hitlagTimer.Init();
             
             stateMachine.TransitionTo(hitlagstate);
             onHitlag.Invoke(durationHitlag);
        }
    }
}
