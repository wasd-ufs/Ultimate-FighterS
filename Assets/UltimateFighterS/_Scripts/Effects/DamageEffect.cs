using UnityEngine;

public class DamageEffect : Effect
{
    [SerializeField] private float damageAmount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<DamageComponent>()?.TakeDamage(damageAmount);
    }
}