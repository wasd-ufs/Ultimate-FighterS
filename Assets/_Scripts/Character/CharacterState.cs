using UnityEngine;

public abstract class CharacterState : State
{
    public StateMachine<CharacterState> machine { get; set; }
    public InputSystem input { get; set; }
    public CharacterBody2D body { get; set; }
    public InputBuffer inputBuffer { get; set; }

    public virtual void Process() {}

    public virtual void PhysicsProcess() {}

    public virtual void OnCeilingEnter(Vector2 normal) {}
    public virtual void OnCeilingExit(Vector2 normal) {}

    public virtual void OnFloorEnter(Vector2 normal) {}
    public virtual void OnFloorExit(Vector2 normal) {}

    public virtual void OnLeftWallEnter(Vector2 normal) {}
    public virtual void OnLeftWallExit(Vector2 normal) {}

    public virtual void OnRightWallEnter(Vector2 normal) {}
    public virtual void OnRightWallExit(Vector2 normal) {}
    
    public enum CharacterBasis
    {
        Floor,
        Wall,
        Ceiling,
        Body,
        XY,
        Input
    }
    
    public (Vector2, Vector2) GetBasis(CharacterBasis basis) => basis switch
    {
        CharacterBasis.Floor => (body.GetFloorRight(), body.FloorNormal),
        CharacterBasis.Wall => body.IsOnLeftWall() ? (body.LeftWallNormal, body.GetLeftWallUp()) : (body.RightWallNormal, body.GetRightWallUp()),
        CharacterBasis.Ceiling => (body.GetCeilingLeft(), body.CeilingNormal),
        CharacterBasis.Body => (body.Right, body.Up),
        CharacterBasis.XY => (Vector2.right, Vector2.up),
        CharacterBasis.Input => (input.GetDirection(), VectorUtils.Orthogonal(input.GetDirection()))
    };
}
