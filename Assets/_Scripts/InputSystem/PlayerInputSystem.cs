using UnityEngine;

public class PlayerInputSystem : InputSystem
{
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private string specialKey = "Jump";
    [SerializeField] private string attackKey = "Fire1";
    public override Vector2 GetDirection()
    {
        float horizontal = Input.GetAxis(horizontalAxis);
        float vertical = Input.GetAxis(verticalAxis);
        return new Vector2(horizontal, vertical).normalized;
    }
    public override bool IsSpecialBeingHeld() 
    {
        return Input.GetButton(specialKey);
    }

    public override bool IsSpecialJustPressed() 
    {
        return Input.GetButtonDown(specialKey);
    }

    public override bool isAttackBeingHeld() 
    {
        return Input.GetButton(attackKey);
    }

    public override bool IsAttackJustPressed() 
    {
        return Input.GetButtonDown(attackKey);
    }
}
