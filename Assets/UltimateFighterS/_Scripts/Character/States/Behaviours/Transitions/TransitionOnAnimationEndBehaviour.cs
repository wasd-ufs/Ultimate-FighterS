using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Realiza uma transição assim que a animação atual acaba
/// </summary>
[RequireComponent(typeof(Animator))]
public class TransitionOnAnimationEndBehaviour : CharacterState
{
    [FormerlySerializedAs("next")] [SerializeField] private CharacterState _next;
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void StateUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !_animator.IsInTransition(0))
            Machine.TransitionTo(_next);
    }
}