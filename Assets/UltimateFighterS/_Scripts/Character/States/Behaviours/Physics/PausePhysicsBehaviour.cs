using UnityEngine;

public class PausePhysicsBehaviour : CharacterState
{
    public override void Enter()
    {
        Body.Pause();
    }

    public override void Exit()
    {
        Body.Resume();
    }
}