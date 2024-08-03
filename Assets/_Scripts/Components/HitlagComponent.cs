using UnityEngine;

public class HitlagComponent : CharacterState
{
      [Header ("References")] 
      [SerializeField] private HitlagState hitlagstate;
      [SerializeField] private StateMachine<CharacterState> stateMachine;


    public void Apply(float durationHitlag)
    {
        if (hitlagstate != null && stateMachine != null) {
             hitlagstate.SetHitlagTime(durationHitlag);
             stateMachine.TransitionTo(hitlagstate);
        }
    }
}
