using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Hurtbox))]
public class HurtboxTransition : CharacterState
{
    [SerializeField] private CharacterState next;
    private Hurtbox hurtbox;

    void Awake()
    {
        hurtbox = GetComponent<Hurtbox>();
        hurtbox.onHitBoxDetected.AddListener(OnHitboxDetected);
    }

    void OnHitboxDetected(GameObject obj)
    {
        Machine.TransitionTo(next);
    }
}
