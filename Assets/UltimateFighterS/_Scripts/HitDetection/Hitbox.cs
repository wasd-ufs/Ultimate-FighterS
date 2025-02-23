using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : OwnedComponent
{
    [SerializeField] public UnityEvent<GameObject> onHurtboxDetected = new();
    private readonly List<GameObject> objectsHitThisFrame = new();

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

    private void OnEnable()
    {
        objectsHitThisFrame.Clear();
    }
}