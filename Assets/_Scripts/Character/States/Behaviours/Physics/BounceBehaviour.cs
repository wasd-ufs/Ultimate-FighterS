using UnityEngine;

public class BounceBehaviour : CharacterState
{
    [Range(0f, 1f)] [SerializeField] public float reductionPerBounce = 0.5f;

    void Bounce(Vector2 normal)
    {
        body.SetVelocity(VectorUtils.Reflected(body.LastVelocity, normal) * (1f - reductionPerBounce));
    }

    public override void PhysicsProcess()
    {
        if (body.IsOnFloor())
        {
            Bounce(body.FloorNormal);
            return;
        }

        if (body.IsOnCeiling())
        {
            Bounce(body.CeilingNormal);
            return;
        }

        if (body.IsOnLeftWall())
        {
            Bounce(body.LeftWallNormal);
            return;
        }

        if (body.IsOnRightWall())
        {
            Bounce(body.RightWallNormal);
            return;
        }
    }
}