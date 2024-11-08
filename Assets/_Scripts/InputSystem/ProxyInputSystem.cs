using UnityEngine;

public class ProxyInputSystem : InputSystem
{
    [SerializeField] public InputSystem input;
    
    public override Vector2 GetDirection() => input?.GetDirection() ?? Vector2.zero;
    public override bool IsSpecialBeingHeld() => input?.IsSpecialBeingHeld() ?? false;
    public override bool IsSpecialJustPressed() => input?.IsSpecialJustPressed() ?? false;
    public override bool IsAttackBeingHeld() => input?.IsAttackBeingHeld() ?? false;
    public override bool IsAttackJustPressed() => input?.IsAttackJustPressed() ?? false;
}