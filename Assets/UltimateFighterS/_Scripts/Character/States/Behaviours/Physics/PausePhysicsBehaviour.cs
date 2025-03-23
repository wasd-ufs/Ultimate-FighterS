/// <summary>
/// Pausa a física do corpo durante a execução desse estado.
/// A física é retomada ao sair
/// </summary>
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