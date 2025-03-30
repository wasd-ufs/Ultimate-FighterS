using UnityEngine;

public class PlayerInputSystem : InputSystem
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    private const string SPECIAL_KEY = "Jump";
    private const string ATTACK_KEY = "Fire";

    public int defaultId = 0;

    private string _portHorizontalAxis;
    private string _portVerticalAxis;
    private string _portSpecialKey;
    private string _portAttackKey;

    private readonly Vector2[] _possibleDirections = {
        Vector2.right, new Vector2(1, -1).normalized, Vector2.down, new Vector2(-1, -1).normalized, Vector2.left,
        new Vector2(-1, 1).normalized, Vector2.up, new Vector2(1, 1).normalized
    };

    public void Awake()
    {
        int id = GetComponent<IdComponent>()?.id ?? defaultId;
        
        _portHorizontalAxis = $"{HORIZONTAL_AXIS}{id}";
        _portVerticalAxis = $"{VERTICAL_AXIS}{id}";
        _portSpecialKey = $"{SPECIAL_KEY}{id}";
        _portAttackKey = $"{ATTACK_KEY}{id}";
    }
    
    public override Vector2 GetDirection()
    {
        float horizontal = Input.GetAxis(_portHorizontalAxis);
        float vertical = Input.GetAxis(_portVerticalAxis);
        Vector2 direction = new Vector2(horizontal, vertical);
        
        if (direction.sqrMagnitude < 0.05f)
            return Vector2.zero;
        
        direction = direction.normalized;
        return direction;
        /*
        var angle = Mathf.Atan2(direction.y, direction.x);

        var section = Mathf.RoundToInt(angle * 8 / (2 * Mathf.PI) + 8) % 8;
        return possibleDirections[section];
        */
    }
    public override bool IsSpecialBeingHeld() 
    {
        return Input.GetButton(_portSpecialKey);
    }

    public override bool IsSpecialJustPressed() 
    {
        return Input.GetButtonDown(_portSpecialKey);
    }

    public override bool IsAttackBeingHeld() 
    {
        return Input.GetButton(_portAttackKey);
    }

    public override bool IsAttackJustPressed() 
    {
        return Input.GetButtonDown(_portAttackKey);
    }
}