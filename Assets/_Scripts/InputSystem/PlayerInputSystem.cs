using UnityEngine;

[RequireComponent(typeof(IdComponent))]
public class PlayerInputSystem : InputSystem
{
    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";
    private const string specialKey = "Jump";
    private const string attackKey = "Fire";

    private IdComponent id;

    private string portHorizontalAxis;
    private string portVerticalAxis;
    private string portSpecialKey;
    private string portAttackKey;

    public void Awake()
    {
        id = GetComponent<IdComponent>();
        
        portHorizontalAxis = $"{horizontalAxis}{id.id}";
        portVerticalAxis = $"{verticalAxis}{id.id}";
        portSpecialKey = $"{specialKey}{id.id}";
        portAttackKey = $"{attackKey}{id.id}";
    }
    
    public override Vector2 GetDirection()
    {
        float horizontal = Input.GetAxis(portHorizontalAxis);
        float vertical = Input.GetAxis(portVerticalAxis);
        return new Vector2(horizontal, vertical).normalized;
    }
    public override bool IsSpecialBeingHeld() 
    {
        return Input.GetButton(portSpecialKey);
    }

    public override bool IsSpecialJustPressed() 
    {
        return Input.GetButtonDown(portSpecialKey);
    }

    public override bool IsAttackBeingHeld() 
    {
        return Input.GetButton(portAttackKey);
    }

    public override bool IsAttackJustPressed() 
    {
        return Input.GetButtonDown(portAttackKey);
    }
}