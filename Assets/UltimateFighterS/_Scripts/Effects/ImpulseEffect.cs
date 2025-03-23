using UnityEngine;

public class ImpulseEffect : Effect
{
    public Vector2 impulse;
    public bool flipWithObject = true;

    public Vector2 Impulse => flipWithObject ? VectorUtils.Sign(transform.lossyScale) * impulse : impulse;

    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.ApplyImpulse(Impulse);
    }
}