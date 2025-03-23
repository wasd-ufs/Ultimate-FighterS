using UnityEngine;

public class SetSpeedEffect : Effect
{
    public Vector2 axis;
    public float speed;

    public bool flipWithObject = true;
    public Vector2 Axis => flipWithObject ? VectorUtils.Sign(transform.lossyScale) * axis : axis;

    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.SetSpeed(Axis.normalized, speed);
    }
}