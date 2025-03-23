using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Realiza uma transição assim que uma Hitbox é entra em contato com a Hurtbox do corpo
/// </summary>
[RequireComponent(typeof(Hurtbox))]
public class HurtboxTransition : CharacterState
{
    [FormerlySerializedAs("next")] [SerializeField] private CharacterState _next;
    private Hurtbox _hurtbox;

    private void Awake()
    {
        _hurtbox = GetComponent<Hurtbox>();
        _hurtbox.onHitBoxDetected.AddListener(OnHitboxDetected);
    }

    private void OnHitboxDetected(GameObject obj)
    {
        Machine.TransitionTo(_next);
    }
}