using System;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> onHurtboxDetected;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox is null || hurtbox.IsInvincible)
            return;
        
        onHurtboxDetected.Invoke(hurtbox.Owner);
    }
}