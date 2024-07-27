using UnityEngine;

public class PlayerInputSystem : InputSystem
{
    [SerializeField] private string specialKey = "Jump";
    [SerializeField] private string attackKey = "Fire1";
    public override Vector2 GetDirection()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        return new Vector2(horizontalAxis, verticalAxis);
    }
    public override bool IsSpecialBeingHeld() 
    {
        return Input.GetButtonDown(specialKey);
    }

    public override bool IsSpecialJustPressed() 
    {
        return Input.GetButton(specialKey);
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

