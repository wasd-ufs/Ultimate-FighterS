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

    private (bool, Vector2) lastTriggerTest = (false, Vector2.zero);

    public override void Enter()
    {
        lastTriggerTest = (CurrentInputTriggers(), input.GetDirection().normalized);
    }

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
            
            lastTriggerTest = (true, input.GetDirection().normalized);
            return;
        }

        if (CurrentInputTriggers())
        {
            lastTriggerTest = (true, input.GetDirection().normalized);
            TransitionToNext();
            return;
        }

        lastTriggerTest = (false, input.GetDirection().normalized);
    }

    public void TransitionToNext()
    {
        machine.TransitionTo(next);
    }

    public bool CurrentInputTriggers() =>
        DirectionTriggers(input.GetDirection().normalized) && (!requireTap || !lastTriggerTest.Item1 || Vector2.Dot(input.GetDirection().normalized, lastTriggerTest.Item2.normalized) >= 0.01f)
                                                && ButtonCombinationTriggers(input.IsAttackJustPressed(), input.IsSpecialJustPressed());

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
        TriggerDirection.Neutral => direction.sqrMagnitude < 0.001f,
        _ => Mathf.Abs(AngleOfDirection(requiredDirection) - Mathf.Atan2(direction.y, direction.x)) <= (expandedTriggerAngle ? 0.9 : 0.45),
    };

    public float AngleOfDirection(TriggerDirection direction) => Mathf.Deg2Rad * direction switch
    {
        TriggerDirection.Up => 90f,
        TriggerDirection.Down => -90f,
        TriggerDirection.Forward => 180f - 180f * IsLookingForward(),
        TriggerDirection.Backward => 180f * IsLookingForward(),
        TriggerDirection.UpForward => 135f - 90f * IsLookingForward(),
        TriggerDirection.UpBackward => 45f + 90f * IsLookingForward(),
        TriggerDirection.DownForward => -135f + 90f * IsLookingForward(),
        TriggerDirection.DownBackward => -45f - 90f * IsLookingForward(),
        _ => 0f,
    };

    public float IsLookingForward() => (Mathf.Sign(transform.lossyScale.x) + 1) / 2;
}