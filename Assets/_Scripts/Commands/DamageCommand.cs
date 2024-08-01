using UnityEngine;

public class DamageCommand : Command
{
    [SerializeField] private float damageAmount;

    public override void Run(GameObject target)
    {
        target.GetComponent<DamageComponent>().TakeDamage(damageAmount);        
    }
}