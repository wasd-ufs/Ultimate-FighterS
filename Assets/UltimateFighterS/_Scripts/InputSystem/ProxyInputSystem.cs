using UnityEngine;

public class ProxyInputSystem : InputSystem
{
    [SerializeField] public InputSystem input;

    public override Vector2 GetDirection()
    {
        return input?.GetDirection() ?? Vector2.zero;
    }

    public override bool IsSpecialBeingHeld()
    {
        return input?.IsSpecialBeingHeld() ?? false;
    }

    public override bool IsSpecialJustPressed()
    {
        return input?.IsSpecialJustPressed() ?? false;
    }

    public override bool IsAttackBeingHeld()
    {
        return input?.IsAttackBeingHeld() ?? false;
    }

    public override bool IsAttackJustPressed()
    {
        return input?.IsAttackJustPressed() ?? false;
    }
}