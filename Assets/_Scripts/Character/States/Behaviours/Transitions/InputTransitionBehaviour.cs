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
    [Header("Buffer")] 
    [SerializeField] public bool useBuffer = true;

    [Header("Direction")]
    [SerializeField] public TriggerDirection requiredDirection;
    [SerializeField] public bool expandedTriggerAngle;
    [SerializeField] public bool requireTap;

    [Header("Button")] 
    [SerializeField] public TriggerButton requiredButton;

    [Header("Transition")] 
    [SerializeField] public CharacterState next;

    private Vector2 lastDirection = Vector2.zero;

    public override void Process()
    {
        Run();
    }

    private void Run()
    {
        if (useBuffer && InputBufferTriggers())
        {
            inputBuffer.Consume();
            TransitionToNext();
            return;
        }

        if (CurrentInputTriggers())
            TransitionToNext();

        lastDirection = input.GetDirection();
    }

    public void TransitionToNext()
    {
        machine.TransitionTo(next);
    }

    public bool CurrentInputTriggers() =>
        DirectionTriggers(input.GetDirection()) && (!requireTap || !DirectionTriggers(lastDirection))
                                                && ButtonCombinationTriggers(input.IsAttackJustPressed(),
                                                    input.IsSpecialJustPressed());

    public bool InputBufferTriggers() => inputBuffer.Current is not null
                                         && DirectionTriggers(inputBuffer.Current.direction)
                                         && ButtonCombinationTriggers(inputBuffer.Current.isAttackJustPressed,
                                             inputBuffer.Current.isSpecialJustPressed);

    public bool ButtonCombinationTriggers(bool attack, bool special) => requiredButton switch
    {
        TriggerButton.None => true,
        TriggerButton.Attack => attack,
        TriggerButton.Special => special,
        TriggerButton.Any => attack || special
    };

    public bool DirectionTriggers(Vector2 direction) => requiredDirection switch
    {
        TriggerDirection.None => true,
        TriggerDirection.Neutral => direction.sqrMagnitude < 0.01f,
        _ => Vector2.Dot(direction, RequiredDirection) >= (expandedTriggerAngle ? 0.001f : 0.999f)
    };

    public Vector2 RequiredDirection => requiredDirection switch
    {
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