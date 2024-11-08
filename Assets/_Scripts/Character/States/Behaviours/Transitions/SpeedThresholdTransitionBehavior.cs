using UnityEngine;

public enum SpeedComparisonType
{
    Higher,
    Lower,
    ApproximatelyEqual,
    ApproximatelyDifferent,
}

public class SpeedThresholdTransitionBehavior : CharacterState
{
    [Header("Axis")]
    public Vector2 axis;
    public CharacterBasis basis;
    
    [Header("Comparison")]
    public float threshold;
    public SpeedComparisonType comparison;
    
    [Header("Transition")]
    public CharacterState next;

    public override void Enter()
    {
        TryTransition();
    }

    public override void PhysicsProcess()
    {
        TryTransition();
    }
    
    private void TryTransition()
    {
        if (ShouldTransition())
            machine.TransitionTo(next);
    }

    private bool ShouldTransition() => comparison switch
    {
        SpeedComparisonType.ApproximatelyEqual => Mathf.Approximately(body.GetSpeedOnAxis(GetFinalAxis()), threshold),
        SpeedComparisonType.ApproximatelyDifferent => !Mathf.Approximately(body.GetSpeedOnAxis(GetFinalAxis()), threshold),
        SpeedComparisonType.Higher => body.GetSpeedOnAxis(GetFinalAxis()) > threshold,
        SpeedComparisonType.Lower => body.GetSpeedOnAxis(GetFinalAxis()) < threshold,
        _ => false
    };

    private Vector2 GetFinalAxis()
    {
        var (forward, up) = GetBasis(basis);
        return axis.x * forward + axis.y * up;
    }
}