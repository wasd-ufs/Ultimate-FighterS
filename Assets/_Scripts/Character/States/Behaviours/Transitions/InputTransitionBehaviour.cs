using UnityEngine;

public enum TriggerDirection
{
    Up,
    Down,
    Backward,
    Forward,
    UpBackward,
    UpForward,
    DownBackward,
    DownForward,
    None
}

public enum TriggerButton
{
    None,
    Attack,
    Special,
    Any
}

public class InputTransitionBehaviour : CharacterState
{
    [Header("Direction")]
    [SerializeField] public TriggerDirection requiredDirection;
    [SerializeField] public bool tolerateDiagonals;
    
    [Header("Button")]
    [SerializeField] public TriggerButton requiredButton;
    
    [Header("Transition")]
    [SerializeField] public CharacterState next;

    public override void Process()
    {
        if (IsButtonTriggered() && IsDirectionTriggered())
            machine.TransitionTo(next);
    }

    public bool IsButtonTriggered() => requiredButton switch
    {
        TriggerButton.None => true,
        TriggerButton.Attack => input.IsAttackJustPressed(),
        TriggerButton.Special => input.IsSpecialJustPressed(),
        TriggerButton.Any => input.IsAttackJustPressed() || input.IsSpecialJustPressed()
    };

    public bool IsDirectionTriggered() => requiredDirection switch
    {
        TriggerDirection.None => true,
        _ => Vector2.Dot(input.GetDirection(), GetDirection()) >= (tolerateDiagonals ? 0.01f : 0.99f)
    };

    public Vector2 GetDirection() => requiredDirection switch
    {
        TriggerDirection.Up => Vector2.up,
        TriggerDirection.Down => Vector2.down,
        TriggerDirection.Backward => Vector2.left,
        TriggerDirection.Forward => Vector2.right,
        TriggerDirection.UpBackward => new Vector2(-1, 1).normalized,
        TriggerDirection.UpForward => new Vector2(1, 1).normalized,
        TriggerDirection.DownBackward => new Vector2(-1, -1).normalized,
        TriggerDirection.DownForward => new Vector2(1, -1).normalized,
        _ => Vector2.zero,
    };
    
}