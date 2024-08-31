using System;
using UnityEngine;

public class KnockbackEffect : Effect
{
    [SerializeField] private Vector2 knockback;
    public bool applyObjectTransform;
    
    public Vector2 Knockback => applyObjectTransform ? transform.TransformVector(knockback) : knockback;

    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(Knockback);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Knockback);
    }
}
