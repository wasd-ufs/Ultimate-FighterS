using UnityEngine.Events;

public class EnterExitBehaviour : CharacterState
{
    public UnityEvent onEnter = new();
    public UnityEvent onExit = new();

    public override void Enter()
    {
        onEnter.Invoke();
    }

    public override void Exit()
    {
        onExit.Invoke();
    }
}