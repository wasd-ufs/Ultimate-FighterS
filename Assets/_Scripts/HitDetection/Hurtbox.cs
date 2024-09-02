using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : OwnedComponent
{
    [SerializeField] private UnityEvent<GameObject> onHitBoxDetected;
    public bool isInvincible;
    
    public void OnHurted(GameObject hitbox)
    {
        onHitBoxDetected.Invoke(hitbox);
    }
}