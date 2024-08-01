using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent onHurtboxDetected;

    private readonly List<GameObject> objectsHitThisFrame = new();

    private void FixedUpdate()
    {
        objectsHitThisFrame.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox is null || hurtbox.IsInvincible || objectsHitThisFrame.Contains(hurtbox.Owner))
            return;
        
        objectsHitThisFrame.Add(hurtbox.Owner);
        onHurtboxDetected.AddListener(() => hurtbox.OnHurted(hurtbox.Owner));
        onHurtboxDetected.Invoke();
        
    }
}