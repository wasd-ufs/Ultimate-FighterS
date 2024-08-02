using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private UnityEvent<GameObject> onHurtboxDetected;
    
    public GameObject Owner => owner;

    private readonly List<GameObject> objectsHitThisFrame = new();
    
    private void FixedUpdate()
    {
        objectsHitThisFrame.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox is null || hurtbox.Owner == Owner || objectsHitThisFrame.Contains(hurtbox.Owner))
            return;
        
        objectsHitThisFrame.Add(hurtbox.Owner);
        onHurtboxDetected.Invoke(hurtbox.Owner);
        hurtbox.OnHurted(Owner);
    }
}