using UnityEngine;
using UnityEngine.Serialization;

public class ImpulseEffect : BaseEffect
{
    public Vector2 impulse;
    public bool flipWithObject = true;

    public Vector2 Impulse => flipWithObject ? VectorUtils.Sign(transform.lossyScale) * impulse : impulse;
    
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.ApplyImpulse(Impulse);
    }
}