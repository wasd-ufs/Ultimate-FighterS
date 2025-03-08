using UnityEngine;

public class PlayerInputSystem : InputSystem
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string SpecialKey = "Jump";
    private const string AttackKey = "Fire";

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
        var _id = GetComponent<IdComponent>()?.id ?? defaultId;
        
        portHorizontalAxis = $"{HorizontalAxis}{_id}";
        portVerticalAxis = $"{VerticalAxis}{_id}";
        portSpecialKey = $"{SpecialKey}{_id}";
        portAttackKey = $"{AttackKey}{_id}";
    }
    
    public override Vector2 GetDirection()
    {
        float _horizontal = Input.GetAxis(portHorizontalAxis);
        float _vertical = Input.GetAxis(portVerticalAxis);
        var _direction = new Vector2(_horizontal, _vertical);
        
        if (_direction.sqrMagnitude < 0.05f)
            return Vector2.zero;
        
        _direction = _direction.normalized;
        return _direction;
        /*
        var angle = Mathf.Atan2(direction.y, direction.x);

        var section = Mathf.RoundToInt(angle * 8 / (2 * Mathf.PI) + 8) % 8;
        return possibleDirections[section];
        */
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