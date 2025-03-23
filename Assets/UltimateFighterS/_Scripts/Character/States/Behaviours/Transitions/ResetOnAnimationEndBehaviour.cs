using UnityEngine;

/// <summary>
/// Retorna a máquina de estados ao estado inicial assim que a animação atual acaba
/// </summary>
[RequireComponent(typeof(Animator))]
public class ResetOnAnimationEndBehaviour : CharacterState
{
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void StateUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !_animator.IsInTransition(0))
            Machine.Reset();
    }
}