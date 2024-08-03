using UnityEngine;

public class KnockbackEffect : Effect
{
    [SerializeField] private Vector2 knockback;

    public override void Apply(GameObject gameObject)
    {
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(knockback);
    }
}
