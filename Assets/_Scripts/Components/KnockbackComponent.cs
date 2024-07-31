using UnityEngine;
using UnityEngine.Events;

public class KnockbackComponent : MonoBehaviour
{
    [SerializeField] private DamageComponent damageComponent;
    [SerializeField] private float baseKnockback;
    public UnityEvent<float> onKnockback;
    
    public void ApplyKnockback()
    {
        float knockbackForce = baseKnockback * damageComponent.currentDamage;
        onKnockback.Invoke(knockbackForce);
    }
}