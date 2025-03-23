using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : OwnedComponent
{
    [SerializeField] public UnityEvent<GameObject> onHurtboxDetected = new();
    private readonly List<GameObject> _objectsHitThisFrame = new();

    private void OnEnable()
    {
        _objectsHitThisFrame.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox is null)
            return;

        if (IsSameAs(hurtbox) || _objectsHitThisFrame.Contains(hurtbox.Owner))
            return;

        _objectsHitThisFrame.Add(hurtbox.Owner);
        if (!hurtbox.isInvincible) onHurtboxDetected.Invoke(hurtbox.Owner);
        hurtbox.OnHurted(Owner);
    }
}