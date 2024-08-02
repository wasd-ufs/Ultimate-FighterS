using UnityEngine;
using UnityEngine.Events;
public class DeathComponent : MonoBehaviour
{
    public UnityEvent onDeath;

    public void Kill()
    {
        onDeath.Invoke();
        Destroy(gameObject);
    }
}