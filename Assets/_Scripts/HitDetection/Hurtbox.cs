using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private bool isInvincible;

    [SerializeField] private UnityEvent onHitBoxDetected;
    public GameObject Owner => owner;
    public bool IsInvincible => isInvincible;

    public void OnHurted(GameObject _owner)
    {
        if (owner == _owner)
        {
            onHitBoxDetected.Invoke();
        }
        return;
    }
}