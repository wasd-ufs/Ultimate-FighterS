using UnityEngine;

public class InstantResetBehaviour : CharacterState
{
    public override void Enter()
    {
        Machine.Reset();
    }
}