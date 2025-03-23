using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] [ReadOnly] public float currentDamage;
    [HideInInspector] public UnityEvent<float> onDamageUpdate = new();

    public float CurrentDamage => currentDamage;

    public void OnDestroy()
    {
        onDamageUpdate.RemoveAllListeners();
    }

    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        onDamageUpdate.Invoke(currentDamage);
    }
}