using UnityEngine;
using UnityEngine.Events;
public class DeathComponent : MonoBehaviour
{
    public static readonly UnityEvent<GameObject> OnPlayerDeath = new();
    [HideInInspector] public UnityEvent onDeath;

    public void Kill()
    {
        onDeath.Invoke();
        OnPlayerDeath.Invoke(gameObject);
        Destroy(gameObject);
    }
}