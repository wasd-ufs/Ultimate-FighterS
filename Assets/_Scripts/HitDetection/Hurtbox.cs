using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private UnityEvent onHitBoxDetected;
    
    public GameObject Owner => owner;

    public void OnHurted(GameObject _owner)
    {
        if (owner == _owner)
            return;
        
        onHitBoxDetected.Invoke();
    }
}