using UnityEngine;

public enum LandType
{
    Floor,
    Wall,
    Ceiling,
    Any
}

public enum LandingType
{
    Entering,
    Exiting
}

public class SurfaceTransitionBehaviour : CharacterState
{
    [SerializeField] public LandType land;
    [SerializeField] public LandingType type;
    [SerializeField] public CharacterState next;

    public override void Enter()
    {
        if (Landed())
            Machine.TransitionTo(next);
    }

    public override void PhysicsProcess()
    {
        if (Landed())
            Machine.TransitionTo(next);
    }

    private bool Landed() =>
        (IsOnLand() && type == LandingType.Entering) || (!IsOnLand() && type == LandingType.Exiting);
    
    public bool IsOnLand() => land switch
    {
        LandType.Floor => Body.IsOnFloor(),
        LandType.Ceiling => Body.IsOnCeiling(),
        LandType.Wall => Body.IsOnLeftWall() || Body.IsOnRightWall(),
        _ => Body.IsOnFloor() || Body.IsOnCeiling() || Body.IsOnLeftWall() || Body.IsOnRightWall(),
    };
}