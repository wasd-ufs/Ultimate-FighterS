using System;
using UnityEngine;
using UnityEngine.Serialization;

public class KnockbackEffect : Effect
{
    [SerializeField] public Knockback Knockback;

    public override void Apply(GameObject gameObject)
    {
        Knockback.direction.x = Mathf.Sign(transform.lossyScale.x) * Mathf.Abs(Knockback.direction.x);
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(Knockback);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Knockback.Impulse(50f));
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Knockback.Impulse(0f));
    }
}
