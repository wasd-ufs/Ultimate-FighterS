using UnityEngine;

public class LimitSpeedEffect : Effect
{
    public Vector2 axis;
    public float maximum;
    
    public bool applyObjectTransform;
    public Vector2 Axis => applyObjectTransform ? transform.TransformVector(axis) : axis;
    
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.LimitSpeed(Axis.normalized, maximum);
    }
}