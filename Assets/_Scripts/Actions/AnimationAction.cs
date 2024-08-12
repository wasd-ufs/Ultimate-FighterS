using UnityEngine;

public class AnimationAction : ActionSystem
{
    [SerializeField] private Animator animator;
    [SerializeField] private string startTrigger = "Start";

    public override void Enter()
    {
        animator.SetTrigger(startTrigger);
    }

    public override void Exit()
    {
        animator.ResetTrigger(startTrigger);
    }
}
