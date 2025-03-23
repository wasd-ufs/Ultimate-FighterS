using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Realiza a transição assim que o timer terminar
/// </summary>
[RequireComponent(typeof(Timer))]
public class TimedTransitionBehaviour : CharacterState
{
    [FormerlySerializedAs("next")] [SerializeField] private CharacterState _next;
    private bool _skip;
    private Timer _timer;

    private void Awake()
    {
        _timer = GetComponent<Timer>();
        _timer.onTimerFinish.AddListener(OnTimerFinish);
        _skip = true;
    }

    public override void Enter()
    {
        _skip = false;
        _timer.Init();

        _timer.enabled = true;
    }

    public override void StateUpdate()
    {
        _timer.Update();
    }

    public override void Exit()
    {
        _skip = true;
        _timer.enabled = false;
    }

    /// <summary>
    /// Callback de quando o Timer acaba.
    /// Realiza a transição
    /// </summary>
    private void OnTimerFinish()
    {
        if (_skip)
            return;

        Machine.TransitionTo(_next);
    }
}