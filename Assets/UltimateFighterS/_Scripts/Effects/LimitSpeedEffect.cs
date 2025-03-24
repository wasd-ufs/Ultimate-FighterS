using UnityEngine;
using UnityEngine.Serialization;

public class LimitSpeedEffect : BaseEffect
{
    public Vector2 axis;
    public float maximum;
    
    public bool flipWithObject = true;
    public Vector2 Axis => flipWithObject ? VectorUtils.Sign(transform.lossyScale) * axis : axis;
    
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.LimitSpeed(Axis.normalized, maximum);
    }
}