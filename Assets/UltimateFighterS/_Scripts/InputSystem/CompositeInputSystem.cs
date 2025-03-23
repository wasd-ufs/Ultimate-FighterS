using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeInputSystem : InputSystem
{
    private List<InputSystem> _subordinates;

    private void Start()
    {
        _subordinates = GetComponents<InputSystem>().ToList()
            .Where(i => i != this).ToList();
    }

    public override Vector2 GetDirection()
    {
        return _subordinates
            .Aggregate(Vector2.zero, (current, subordinate) => current + subordinate.GetDirection())
            .normalized;
    }

    public override bool IsSpecialBeingHeld()
    {
        return _subordinates.Any(s => s.IsSpecialBeingHeld());
    }

    public override bool IsSpecialJustPressed()
    {
        return _subordinates.Any(s => s.IsSpecialJustPressed());
    }

    public override bool IsAttackBeingHeld()
    {
        return _subordinates.Any(s => s.IsAttackBeingHeld());
    }

    public override bool IsAttackJustPressed()
    {
        return _subordinates.Any(s => s.IsAttackJustPressed());
    }
}