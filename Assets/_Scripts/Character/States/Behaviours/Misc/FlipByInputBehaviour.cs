using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipByInputBehaviour : CharacterState
{
    public override void Process()
    {
        if ((IsFacingRight() && body.GetSpeedRight() < 0) || (!IsFacingRight() && body.GetSpeedLeft() < 0))
            Flip();
    }

    private void Flip()
    {
        Vector2 scale = FlipPivotPoint.localScale;
        scale.x *= -1;
        FlipPivotPoint.localScale = scale;
    }

    private bool IsFacingRight() => FlipPivotPoint.localScale.x > 0;
}
