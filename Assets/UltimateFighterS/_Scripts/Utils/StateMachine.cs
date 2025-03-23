using UnityEngine;

public abstract class StateMachine<TS> : MonoBehaviour where TS : State
{
    [SerializeField] public TS initialState;

    public TS Current { get; protected set; }

    public void Reset()
    {
        TransitionTo(initialState);
    }

    private void Start()
    {
        Reset();
        OnStart();
    }

    public void TransitionTo(TS next)
    {
        Current?.Exit();

        Current = next;
        OnTransition();

        Current.Enter();
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnTransition()
    {
    }
}

public abstract class State : MonoBehaviour
{
    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }
}