using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterBody2D))]
[RequireComponent(typeof(DamageComponent))]
public class KnockbackComponent : MonoBehaviour
{
    private CharacterBody2D body;
    private DamageComponent damageComponent;
    
    [HideInInspector] public UnityEvent<Knockback> onKnockback = new();

    public void Awake()
    {
        body = GetComponent<CharacterBody2D>();
        damageComponent = GetComponent<DamageComponent>();
    }

    public void ApplyKnockback(Knockback knockback)
    {
        body.SetVelocity(knockback.Impulse(damageComponent.CurrentDamage));
        
        Debug.Log(body.Velocity);
        
        onKnockback.Invoke(knockback);
    }
}

[Serializable]
public class Knockback
{
    public Vector2 direction;
    public float setKnockback;
    public float knockbackScaling;

    public Vector2 Impulse(float damage) =>
        direction.normalized * (setKnockback + knockbackScaling * DamageToImpulseMultiplier(damage));
    
    public float DamageToImpulseMultiplier(float damage) => 0.5f
        + damage * 0.0383f
        + damage * damage * 0.0001f
        + damage * damage * damage * -0.000001333f;
}