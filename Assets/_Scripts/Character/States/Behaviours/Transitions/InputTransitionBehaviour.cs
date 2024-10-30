using UnityEngine;

public enum TriggerDirection
{
    Neutral,
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
        TriggerDirection.Neutral => input.GetDirection().sqrMagnitude <= 0.001f,
        _ => Vector2.Dot(input.GetDirection(), GetDirection()) >= (tolerateDiagonals ? 0.01f : 0.99f)
    };

    public Vector2 GetDirection() => requiredDirection switch
    {
        TriggerDirection.Neutral => Vector2.zero,
        TriggerDirection.Up => Vector2.up,
        TriggerDirection.Down => Vector2.down,
        TriggerDirection.Backward => Vector2.left * Mathf.Sign(transform.lossyScale.x),
        TriggerDirection.Forward => Vector2.right * Mathf.Sign(transform.lossyScale.x),
        TriggerDirection.UpBackward => new Vector2(-Mathf.Sign(transform.lossyScale.x), 1).normalized,
        TriggerDirection.UpForward => new Vector2(Mathf.Sign(transform.lossyScale.x), 1).normalized,
        TriggerDirection.DownBackward => new Vector2(-Mathf.Sign(transform.lossyScale.x), -1).normalized,
        TriggerDirection.DownForward => new Vector2(Mathf.Sign(transform.lossyScale.x), -1).normalized,
        _ => Vector2.zero,
    };
    
}