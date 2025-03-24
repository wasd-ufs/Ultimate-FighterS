using System;
using UnityEngine;
using UnityEngine.Serialization;

public class KnockbackEffect : BaseEffect
{
    [SerializeField] public Knockback Knockback;

    public override void Apply(GameObject gameObject)
    {
        var direction = new Vector2(Mathf.Sign(transform.lossyScale.x) * Knockback.direction.x, Knockback.direction.y);

        var finalKnockback = new Knockback(direction, Knockback.setKnockback, Knockback.knockbackScaling);
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(finalKnockback);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Knockback.Impulse(50f));
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Knockback.Impulse(0f));
    }
}
