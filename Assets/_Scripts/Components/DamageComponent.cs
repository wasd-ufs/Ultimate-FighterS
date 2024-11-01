using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] [ReadOnly] private float currentDamage = 0f;
    [HideInInspector] public UnityEvent<float> onDamageUpdate;
    
    public float CurrentDamage => currentDamage;

    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        onDamageUpdate.Invoke(damage);
    }
}