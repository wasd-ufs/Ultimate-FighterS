using UnityEngine;

public class PausePhysicsBehaviour : CharacterState
{
    public override void Enter()
    {
        body.Pause();
    }

    public override void Exit()
    {
        body.Resume();
    }
}