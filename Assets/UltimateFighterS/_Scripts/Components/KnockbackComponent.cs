using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterBody2D))]
[RequireComponent(typeof(DamageComponent))]
public class KnockbackComponent : MonoBehaviour
{
    [SerializeField] private float knockbackMultiplier = 1f;
    [HideInInspector] public UnityEvent<Knockback> onKnockback = new();
    private CharacterBody2D _body;
    private DamageComponent _damageComponent;

    public void Awake()
    {
        _body = GetComponent<CharacterBody2D>();
        _damageComponent = GetComponent<DamageComponent>();
    }

    public void ApplyKnockback(Knockback knockback)
    {
        _body.SetVelocity(knockback.Impulse(_damageComponent.CurrentDamage) * knockbackMultiplier);

        Debug.Log(_body.Velocity);

        onKnockback.Invoke(knockback);
    }
}

[Serializable]
public class Knockback
{
    public Vector2 direction;
    public float setKnockback;
    public float knockbackScaling;

    public Knockback(Vector2 direction, float setKnockback, float knockbackScaling)
    {
        this.direction = direction.normalized;
        this.setKnockback = setKnockback;
        this.knockbackScaling = knockbackScaling;
    }

    public Vector2 Impulse(float damage)
    {
        return direction.normalized * (setKnockback + knockbackScaling * DamageToImpulseMultiplier(damage));
    }

    public float DamageToImpulseMultiplier(float damage)
    {
        return 0.5f
               + damage * 0.0383f
               + damage * damage * 0.0001f
               + damage * damage * damage * -0.000001333f;
    }
}