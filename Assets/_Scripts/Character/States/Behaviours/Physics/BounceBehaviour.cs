using UnityEngine;

public class BounceBehaviour : CharacterState
{
    [Range(0f, 1f)]
    [SerializeField] public float reductionPerBounce = 0.5f;

    void Bounce(Vector2 normal)
    {
        body.SetVelocity(VectorUtils.Reflected(body.LastVelocity, normal) * (1f - reductionPerBounce));
    }

    public override void OnFloorEnter(Vector2 normal)
    {
        Bounce(normal);
    }

    public override void OnLeftWallEnter(Vector2 normal)
    {
        Bounce(normal);
    }

    public override void OnRightWallEnter(Vector2 normal)
    {
        Bounce(normal);
    }

    public override void OnCeilingEnter(Vector2 normal)
    {
        Bounce(normal);
    }
}