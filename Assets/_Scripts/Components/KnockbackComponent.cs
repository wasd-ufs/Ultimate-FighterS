using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterBody2D))]
[RequireComponent(typeof(DamageComponent))]
public class KnockbackComponent : MonoBehaviour
{
    private CharacterBody2D body;
    private DamageComponent damageComponent;
    
    [HideInInspector] public UnityEvent<Vector2> onKnockback;

    public void Awake()
    {
        body = GetComponent<CharacterBody2D>();
        damageComponent = GetComponent<DamageComponent>();
    }

    public void ApplyKnockback(Vector2 knockback)
    {
        body.SkipSnappingFrame();
        knockback *= KnockbackMultiplier(damageComponent.CurrentDamage);
        body.SetVelocity(knockback);
        onKnockback.Invoke(knockback);
    }

    public Vector2 KnockbackMultiplier(float damage) => new(
        0.4f + damage * 0.034f + damage * damage * 0.00001f - damage * damage * damage * 0.00000012f,
        0.6f + damage * 0.056f + damage * damage * 0.00018f - damage * damage * damage * 0.0000002f
    );
}