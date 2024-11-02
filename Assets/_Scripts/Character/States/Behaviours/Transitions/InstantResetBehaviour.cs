using UnityEngine;

public class InstantResetBehaviour : CharacterState
{
    public override void Enter()
    {
        machine.Reset();
    }
}