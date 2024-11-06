using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] [ReadOnly] public float currentDamage = 0f;
    [HideInInspector] public UnityEvent<float> onDamageUpdate = new();
    
    public float CurrentDamage => currentDamage;

    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        onDamageUpdate.Invoke(currentDamage);
    }

    public void OnDestroy()
    {
        onDamageUpdate.RemoveAllListeners();
    }
}