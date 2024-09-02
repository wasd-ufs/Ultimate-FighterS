using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : OwnedComponent
{
    [SerializeField] private UnityEvent<GameObject> onHurtboxDetected;
    private readonly List<GameObject> objectsHitThisFrame = new();

    private void FixedUpdate()
    {
        objectsHitThisFrame.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox is null)
            return;
        
        if (IsSameAs(hurtbox) || objectsHitThisFrame.Contains(hurtbox.Owner))
            return;
        
        objectsHitThisFrame.Add(hurtbox.Owner);
        if (!hurtbox.isInvincible) onHurtboxDetected.Invoke(hurtbox.Owner);
        hurtbox.OnHurted(Owner);
    }
}