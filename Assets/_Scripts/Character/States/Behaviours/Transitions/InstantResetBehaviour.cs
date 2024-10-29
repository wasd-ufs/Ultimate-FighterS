using UnityEngine;

public class InstantResetBehaviour : CharacterState
{
    public override void Process()
    {
        machine.Reset();
    }
}