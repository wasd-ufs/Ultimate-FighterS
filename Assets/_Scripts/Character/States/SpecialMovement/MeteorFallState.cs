using Unity.VisualScripting;
using UnityEngine;

public class MeteorFallState : CharacterState
{
    [SerializeField] private float fallSpeed = 10f;

    public override void Enter()
    {
        body.SetVelocity(fallSpeed * body.Down);
    }
}