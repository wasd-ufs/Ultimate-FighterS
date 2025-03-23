/// <summary>
/// Faz a maquina voltar para o estado inicial imediatamente ao entrar no estado
/// </summary>
public class InstantResetBehaviour : CharacterState
{
    public override void Enter()
    {
        Machine.Reset();
    }
}