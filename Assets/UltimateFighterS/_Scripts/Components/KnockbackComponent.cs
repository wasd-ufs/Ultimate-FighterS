using System;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Controla a aplicação de knockback ao receber dano.
///</summary>
[RequireComponent(typeof(CharacterBody2D))]
[RequireComponent(typeof(DamageComponent))]
public class KnockbackComponent : MonoBehaviour
{
    private CharacterBody2D _body;
    private DamageComponent _damageComponent;
    [SerializeField] private float _knockbackMultiplier = 1f;
    [HideInInspector] public UnityEvent<Knockback> onKnockback = new();

    public void Awake()
    {
        _body = GetComponent<CharacterBody2D>();
        _damageComponent = GetComponent<DamageComponent>();
    }

    ///<summary>
    /// Aplica o knockback
    ///<param name="knockback"> A quantidade de knockback a ser aplicada.
    ///</summary>
    ///<author>Davi Fontes</author>
    public void ApplyKnockback(Knockback knockback)
    {
        _body.SetVelocity(knockback.Impulse(_damageComponent.CurrentDamage) * _knockbackMultiplier);
        
        onKnockback.Invoke(knockback);
    }
}

///<summary>
/// Representa o efeito de knockback aplicado a um personagem.
///</summary>
[Serializable]
public class Knockback
{
    public Vector2 Direction { get; private set; }
    public float SetKnockback { get; private set; }
    public float KnockbackScaling { get; private set; }

    public Knockback(Vector2 direction, float setKnockback, float knockbackScaling)
    {
        this.Direction = direction.normalized;
        this.SetKnockback = setKnockback;
        this.KnockbackScaling = knockbackScaling;
    }

    ///<summary>
    /// Calcula o vetor de impulso baseado no dano recebido.
    ///</summary>
    ///<param name="damage">Dano recebido.</param>
    ///<returns>Vetor de impulso resultante.</returns>
    ///<author>Davi Fontes</author>
    public Vector2 Impulse(float damage) =>
        Direction.normalized * (SetKnockback + KnockbackScaling * DamageToImpulseMultiplier(damage));

    ///<summary>
    /// Converte o dano recebido em um multiplicador de impulso usando uma fórmula.
    ///</summary>
    ///<param name="damage">Dano recebido.</param>
    ///<returns>Multiplicador aplicado ao knockback.</returns>
    ///<author>Davi Fontes</author>
    public float DamageToImpulseMultiplier(float damage) => 0.5f
        + damage * 0.0383f
        + damage * damage * 0.0001f
        + damage * damage * damage * -0.000001333f;
}