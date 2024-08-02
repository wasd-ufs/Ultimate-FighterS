using UnityEngine;

public class KnockbackCommand : Command
{
    [SerializeField] private Vector2 knockback;

    public override void Run(GameObject gameObject)
    {
        gameObject.GetComponent<KnockbackComponent>()?.ApplyKnockback(knockback);
    }
}
