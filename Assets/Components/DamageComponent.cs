using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour
{
    public float currentDamage;
    public UnityEvent<float> onDamageUpdate;
    
    public float CurrentDamage => currentDamage;

    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        onDamageUpdate.Invoke(damage);
    }
}