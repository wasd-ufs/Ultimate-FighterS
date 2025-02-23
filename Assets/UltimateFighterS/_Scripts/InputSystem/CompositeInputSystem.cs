using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeInputSystem : InputSystem
{
    private List<InputSystem> subordinates;

    private void Start()
    {
        subordinates = GetComponents<InputSystem>().ToList()
            .Where(i => i != this).ToList();
    }

    public override Vector2 GetDirection() => subordinates
        .Aggregate(Vector2.zero, (current, subordinate) => current + subordinate.GetDirection())
        .normalized;

    public override bool IsSpecialBeingHeld() => subordinates.Any(s => s.IsSpecialBeingHeld());

    public override bool IsSpecialJustPressed() => subordinates.Any(s => s.IsSpecialJustPressed());

    public override bool IsAttackBeingHeld() => subordinates.Any(s => s.IsAttackBeingHeld());

    public override bool IsAttackJustPressed() => subordinates.Any(s => s.IsAttackJustPressed());
}