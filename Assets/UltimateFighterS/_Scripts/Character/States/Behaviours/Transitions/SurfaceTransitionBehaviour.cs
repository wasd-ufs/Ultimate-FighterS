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
            machine.TransitionTo(next);
    }

    public override void PhysicsProcess()
    {
        if (Landed())
            machine.TransitionTo(next);
    }

    private bool Landed() =>
        (IsOnLand() && type == LandingType.Entering) || (!IsOnLand() && type == LandingType.Exiting);
    
    public bool IsOnLand() => land switch
    {
        LandType.Floor => body.IsOnFloor(),
        LandType.Ceiling => body.IsOnCeiling(),
        LandType.Wall => body.IsOnLeftWall() || body.IsOnRightWall(),
        _ => body.IsOnFloor() || body.IsOnCeiling() || body.IsOnLeftWall() || body.IsOnRightWall(),
    };
}