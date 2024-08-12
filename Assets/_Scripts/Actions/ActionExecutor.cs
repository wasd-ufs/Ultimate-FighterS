
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ActionExecutor : StateMachine<ActionSystem>
{
    public UnityEvent OnActionEnter;
    public UnityEvent OnActionExit;
    
    public bool IsRunning()
    {
        return Current != null;
    }

    public void TryFinish()
    {
        Current?.OnFinishRequested();
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