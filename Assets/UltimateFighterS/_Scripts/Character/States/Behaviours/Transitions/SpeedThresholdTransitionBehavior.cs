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

    private float lastSpeed;
    
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
            Machine.TransitionTo(next);

        lastSpeed = Body.GetSpeedOnAxis(GetFinalAxis());
    }

    private bool ShouldTransition() => comparison switch
    {
        SpeedComparisonType.ApproximatelyEqual => Approximately(Body.GetSpeedOnAxis(GetFinalAxis()), threshold),
        SpeedComparisonType.ApproximatelyDifferent => !Approximately(Body.GetSpeedOnAxis(GetFinalAxis()), threshold),
        SpeedComparisonType.Higher => Body.GetSpeedOnAxis(GetFinalAxis()) > threshold,
        SpeedComparisonType.Lower => Body.GetSpeedOnAxis(GetFinalAxis()) < threshold,
        _ => false
    };

    private Vector2 GetFinalAxis()
    {
        var (forward, up) = GetBasis(basis);
        return axis.x * forward + axis.y * up;
    }
    
    private bool Approximately(float a, float b) => Mathf.Abs(a - b) < 0.01f;
}