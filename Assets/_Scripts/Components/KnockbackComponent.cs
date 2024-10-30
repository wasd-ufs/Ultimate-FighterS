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
        knockback = knockback.normalized *
                    (knockback.magnitude + Mathf.Max(0f, Vector2.Dot(knockback.normalized, body.Velocity)));
        
        knockback *= KnockbackMultiplier(damageComponent.CurrentDamage);
        body.SetVelocity(knockback);
        onKnockback.Invoke(knockback);
    }

    public Vector2 KnockbackMultiplier(float damage) => new(
        0.4f + 0.01f * damage,
        0.1f + 0.004f * damage
    );
}