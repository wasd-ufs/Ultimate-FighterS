using UnityEngine;

public class DamageEffect : BaseEffect
{
    [SerializeField] private float damageAmount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<DamageComponent>()?.TakeDamage(damageAmount);        
    }
}