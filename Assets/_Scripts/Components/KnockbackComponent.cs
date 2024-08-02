using UnityEngine;
using UnityEngine.Events;

public class KnockbackComponent : MonoBehaviour
{
    [SerializeField] private CharacterBody2D body;
    [SerializeField] private DamageComponent damageComponent;
    
    public UnityEvent onKnockback;
    
    public void ApplyKnockback(Vector2 knockback)
    {
        knockback *= KnockbackMultiplier(damageComponent.CurrentDamage);
        body.ApplyImpulse(knockback);
        onKnockback.Invoke();
    }

    float KnockbackMultiplier(float damage) => 1.0f + damage * 0.05f;
}