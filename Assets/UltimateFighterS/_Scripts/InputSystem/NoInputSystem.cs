using UnityEngine;

public class NoInputSystem : InputSystem
{
    public override Vector2 GetDirection()
    {
        return Vector2.zero;
    }

    public override bool IsSpecialBeingHeld()
    {
        return false;
    }

    public override bool IsSpecialJustPressed()
    {
        return false;
    }

    public override bool IsAttackBeingHeld()
    {
        return false;
    }

    public override bool IsAttackJustPressed()
    {
        return false;
    }
}