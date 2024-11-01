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

    public float KnockbackMultiplier(float damage) => 1.12f + damage * 0.005f - Mathf.Exp(-damage * 0.0025f);
}