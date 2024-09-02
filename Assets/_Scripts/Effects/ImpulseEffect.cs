using UnityEngine;

public class ImpulseEffect : Effect
{
    public Vector2 impulse;
    public bool applyObjectTransform;

    public Vector2 Impulse => applyObjectTransform ? transform.TransformVector(impulse) : impulse;
    
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.ApplyImpulse(Impulse);
    }
}