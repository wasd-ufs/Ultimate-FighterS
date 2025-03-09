using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

///<summary>
/// Controla o estado de Hitlag do jogador.
///</summary>
public class HitlagComponent : CharacterState
{
    [SerializeField] private CharacterState _hitlagState;
    [SerializeField] private CharacterState _hitstopState;
    
    [HideInInspector] public UnityEvent<float> onHitlag = new();
    
    private Timer _hitlagTimer;
    private Timer _hitstopTimer;
    
    private StateMachine<CharacterState> _stateMachine;
    
    public void Start()
    {
        _hitlagTimer = _hitlagState.GetComponent<Timer>();
        _hitstopTimer = _hitstopState.GetComponent<Timer>();
        
        _stateMachine = GetComponent<StateMachine<CharacterState>>();
    }

    ///<summary>
    /// Aplica o efeito de Hitlag pelo tempo definido.
    ///</summary>
    ///<param name="durationHitlag"> A duração do Hitlag
    ///<param name="durationHitstop">A duração do Hitstop
    ///<author>Davi Fontes</author>
    public void Apply(float durationHitlag, float durationHitstop)
    {
        if (_hitlagState != null && _hitstopTimer != null && _stateMachine != null)
        {
            _hitlagTimer.waitTime = durationHitlag;
            _hitstopTimer.waitTime = durationHitstop;
            
            _stateMachine.TransitionTo(_hitstopState);
            onHitlag.Invoke(durationHitlag);
        }
    }

    public void OnDestroy()
    {
        onHitlag.RemoveAllListeners();
    }
}
