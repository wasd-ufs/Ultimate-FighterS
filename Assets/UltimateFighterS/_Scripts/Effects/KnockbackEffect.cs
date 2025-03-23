using UnityEngine;
using UnityEngine.Serialization;

public class KnockbackEffect : Effect
{
    [FormerlySerializedAs("Knockback")] [SerializeField] public Knockback knockback;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, knockback.Impulse(50f));

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, knockback.Impulse(0f));
    }

    public override void Apply(GameObject gameObject)
    {
        Vector2 direction = new(Mathf.Sign(transform.lossyScale.x) * knockback.direction.x, knockback.direction.y);

        Knockback finalKnockback = new(direction, knockback.setKnockback, knockback.knockbackScaling);
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(finalKnockback);
    }
}