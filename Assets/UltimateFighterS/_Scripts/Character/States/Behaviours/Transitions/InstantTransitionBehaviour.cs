using UnityEngine;

public class InstantTransitionBehaviour : CharacterState
{
    [SerializeField] public CharacterState next;

    public override void Enter()
    {
        Machine.TransitionTo(next);
    }
}