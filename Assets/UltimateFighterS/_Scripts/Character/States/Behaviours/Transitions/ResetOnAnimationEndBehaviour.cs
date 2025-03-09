using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ResetOnAnimationEndBehaviour : CharacterState
{
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Process()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !animator.IsInTransition(0))
            Machine.Reset();
    }
}