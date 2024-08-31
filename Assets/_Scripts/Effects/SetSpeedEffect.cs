using UnityEngine;

public class SetSpeedEffect : Effect
{
    public Vector2 axis;
    public float speed;
    
    public bool applyObjectTransform;
    public Vector2 Axis => applyObjectTransform ? transform.TransformVector(axis) : axis;
    
    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterBody2D>()?.SetSpeed(Axis.normalized, speed);
    }
}