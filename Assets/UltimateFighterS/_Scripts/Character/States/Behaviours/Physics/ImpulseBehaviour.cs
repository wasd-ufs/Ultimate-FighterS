using UnityEngine;

public enum MomentumPreservation
{
    None,
    KeepImpulseTangent,
    KeepImpulseDirection,
    Keep
}

public class ImpulseBehaviour : CharacterState
{
    [Header("Impulse")]
    public Vector2 impulse;
    
    [Header("Configuration")]
    public CharacterBasis impulseBasis;
    public MomentumPreservation momentumPreservation;

    public override void Enter()
    {
        var impulse = GetFinalImpulse();
        ApplyMomentumPreservation(impulse);
        Body.ApplyImpulse(impulse);
        
        Body.FixedUpdate();
    }

    private Vector2 GetFinalImpulse()
    {
        var (forward, up) = GetBasis(impulseBasis);
        return impulse.x * forward + impulse.y * up;
    }

    private void ApplyMomentumPreservation(Vector2 impulse)
    {
        switch (momentumPreservation)
        {
            case MomentumPreservation.None:
                Body.SetVelocity(Vector2.zero);
                break;
            
            case MomentumPreservation.KeepImpulseTangent:
                Body.SetSpeed(impulse.normalized, 0f);
                break;
            
            case MomentumPreservation.KeepImpulseDirection:
                Body.SetSpeed(VectorUtils.Orthogonal(impulse).normalized, 0f);
                break;
        }
    }
}