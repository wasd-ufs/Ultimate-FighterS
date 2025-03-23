using UnityEngine;
using UnityEngine.Events;

public class HitlagComponent : CharacterState
{
    [SerializeField] private CharacterState hitlagstate;
    [SerializeField] private CharacterState hitstopstate;

    [HideInInspector] public UnityEvent<float> onHitlag = new();

    private Timer _hitlagTimer;
    private Timer _hitstopTimer;

    private StateMachine<CharacterState> _stateMachine;

    public void Start()
    {
        _hitlagTimer = hitlagstate.GetComponent<Timer>();
        _hitstopTimer = hitstopstate.GetComponent<Timer>();

        _stateMachine = GetComponent<StateMachine<CharacterState>>();
    }

    public void OnDestroy()
    {
        onHitlag.RemoveAllListeners();
    }

    public void Apply(float durationHitlag, float durationHitstop)
    {
        if (hitlagstate != null && _hitstopTimer != null && _stateMachine != null)
        {
            _hitlagTimer.waitTime = durationHitlag;
            _hitstopTimer.waitTime = durationHitstop;

            _stateMachine.TransitionTo(hitstopstate);
            onHitlag.Invoke(durationHitlag);
        }
    }
}