using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationAction : ActionSystem
{
    private Animator animator;
    public string animationName = "Animation";

    private void Start()
    {
        animator = GetComponent<Animator>();
        Exit();
    }

    public override void Enter()
    {
        gameObject.SetActive(true);
        animator.Play(animationName, -1, 0f);
    }

    public override void Exit()
    {
        animator.Play(animationName, -1, 1f);
        gameObject.SetActive(false);
    }
}
