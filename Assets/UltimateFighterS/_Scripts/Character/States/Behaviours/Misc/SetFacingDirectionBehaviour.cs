using UnityEngine;

public enum FacingDirection
{
    Left,
    Right,
    WallNormal,
    Flip
}

public class SetFacingDirectionBehaviour : CharacterState
{
    [SerializeField] private FacingDirection facingDirection;

    public override void Enter()
    {
        if (IsFacingAway())
            Flip();
    }
    
    private bool IsFacingAway() => facingDirection switch
    {
        FacingDirection.Left => FlipPivotPoint.localScale.x > 0,
        FacingDirection.Right => FlipPivotPoint.localScale.x < 0,
        FacingDirection.WallNormal => GetWallNormal().x * FlipPivotPoint.localScale.x < 0,
        FacingDirection.Flip => true
    };

    private Vector2 GetWallNormal() => (Body.IsOnLeftWall())
        ? Body.LeftWallNormal
        : Body.RightWallNormal;
    
    private void Flip()
    {
        Vector2 scale = FlipPivotPoint.localScale;
        scale.x *= -1;
        FlipPivotPoint.localScale = scale;
    }
}