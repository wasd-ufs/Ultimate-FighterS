using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEndTransitionBehaviour : CharacterState
{
    [SerializeField] private CharacterState next;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Process()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !animator.IsInTransition(0))
            machine.TransitionTo(next);
    }
}