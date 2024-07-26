using UnityEngine;

public class PlayerInputSystem : InputSystem
{

    public override Vector2 GetDirection()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        return new Vector2(horizontalAxis, verticalAxis);
    }
    public override bool IsSpecialBeingHeld() 
    {
    return Input.GetButtonDown("Jump");
    }

    public override bool IsSpecialJustPressed() 
    {
    return Input.GetButton("Jump");
    }

    public override bool isAtackBeingHeld() 
    {
    return Input.GetButton("Attack");
    }

    public override bool IsAttackJustPressed() 
    {
    return Input.GetButtonDown("Attack");
    }
}

