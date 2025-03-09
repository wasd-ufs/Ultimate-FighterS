using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TransitionOnAnimationEndBehaviour : CharacterState
{
    private Animator animator;
    [SerializeField] private CharacterState next;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Process()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !animator.IsInTransition(0))
            Machine.TransitionTo(next);
    }
}