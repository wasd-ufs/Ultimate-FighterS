using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInputSystem : InputSystem
{
    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";
    private const string specialKey = "Jump";
    private const string attackKey = "Fire";

    public int defaultId = 0;

    private string portHorizontalAxis;
    private string portVerticalAxis;
    private string portSpecialKey;
    private string portAttackKey;

    private readonly Vector2[] possibleDirections = {
        Vector2.right, new Vector2(1, -1).normalized, Vector2.down, new Vector2(-1, -1).normalized, Vector2.left,
        new Vector2(-1, 1).normalized, Vector2.up, new Vector2(1, 1).normalized
    };

    public void Awake()
    {
        var id = GetComponent<IdComponent>()?.id ?? defaultId;
        
        portHorizontalAxis = $"{horizontalAxis}{id}";
        portVerticalAxis = $"{verticalAxis}{id}";
        portSpecialKey = $"{specialKey}{id}";
        portAttackKey = $"{attackKey}{id}";
    }
    
    public override Vector2 GetDirection()
    {
        float horizontal = Input.GetAxis(portHorizontalAxis);
        float vertical = Input.GetAxis(portVerticalAxis);
        var direction = new Vector2(horizontal, vertical);
        
        if (direction.sqrMagnitude < 0.05f)
            return Vector2.zero;
        
        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x);

        var section = Mathf.RoundToInt(angle * 8 / (2 * Mathf.PI) + 8) % 8;
        return possibleDirections[section];
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