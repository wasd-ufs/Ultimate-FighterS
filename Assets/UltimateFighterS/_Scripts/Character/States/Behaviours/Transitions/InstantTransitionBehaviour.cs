using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Realiza uma transição imediatamente ao entrar no estado
/// </summary>
public class InstantTransitionBehaviour : CharacterState
{
    [FormerlySerializedAs("next")] [SerializeField] private CharacterState _next;

    public override void Enter()
    {
        Machine.TransitionTo(_next);
    }
}