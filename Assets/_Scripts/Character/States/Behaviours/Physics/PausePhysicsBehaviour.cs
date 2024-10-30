using UnityEngine;

public class PausePhysicsBehaviour : CharacterState
{
    private Vector2 enterVelocity;

    public override void Enter()
    {
        enterVelocity = body.Velocity;
        body.Pause();
    }

    public override void Exit()
    {
        body.Resume();
        body.SetVelocity(enterVelocity);
    }
}