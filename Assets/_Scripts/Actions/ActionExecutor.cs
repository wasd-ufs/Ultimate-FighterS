
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ActionExecutor : StateMachine<ActionSystem>
{
    public UnityEvent OnActionEnter;
    public UnityEvent OnActionExit;
    
    public bool IsRunning()
    {
        return Current is not null && Current != initialState;
    }

    public void TryFinish()
    {
        Current?.OnFinishRequested();
    }

    public void ForceFinish()
    {
        TransitionTo(initialState);
    }

    public void TryRun(ActionSystem action)
    {
        TryFinish();
        if (!IsRunning())
        {
            TransitionTo(action);
        }
    }
    protected override void OnTransition()
    {
        if (Current is not null)
        {
            OnActionEnter.Invoke();
            Current.machine = this;
        }
        else
        {
            OnActionExit.Invoke();
        }
    }
}