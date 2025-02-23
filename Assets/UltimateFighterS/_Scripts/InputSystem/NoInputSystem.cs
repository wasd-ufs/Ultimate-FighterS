using UnityEngine;

public class NoInputSystem : InputSystem
{
    public override Vector2 GetDirection() => Vector2.zero;

    public override bool IsSpecialBeingHeld() => false;

    public override bool IsSpecialJustPressed() => false;

    public override bool IsAttackBeingHeld() => false;

    public override bool IsAttackJustPressed() => false;
}