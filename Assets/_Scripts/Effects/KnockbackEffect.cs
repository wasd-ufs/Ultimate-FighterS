using System;
using UnityEngine;
using UnityEngine.Serialization;

public class KnockbackEffect : Effect
{
    [SerializeField] private Vector2 knockback;
    public bool flipWithObject = true;
    
    public Vector2 Knockback => flipWithObject ? VectorUtils.Sign(transform.lossyScale) * knockback : knockback;

    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(Knockback);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Knockback);
    }
}
