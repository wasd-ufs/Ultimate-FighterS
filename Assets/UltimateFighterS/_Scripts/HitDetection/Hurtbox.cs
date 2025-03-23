using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : OwnedComponent
{
    [SerializeField] public UnityEvent<GameObject> onHitBoxDetected = new();
    public bool isInvincible;

    public void OnHurted(GameObject hitbox)
    {
        onHitBoxDetected.Invoke(hitbox);
    }
}