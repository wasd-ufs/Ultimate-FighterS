using System;
using UnityEngine;

public class VectorUtils
{
    public static Vector2 OnAxis(Vector2 axis, Vector2 toModify, Func<float, float> modification)
    {
        if (axis.sqrMagnitude < float.Epsilon)
            return toModify;
        
        var orthogonal = new Vector2(-axis.y, axis.x);
        var onBasis = new Vector2(
            Vector2.Dot(toModify, axis),
            Vector2.Dot(toModify, orthogonal)
        );

        onBasis.x = modification(onBasis.x);
        return onBasis.x * axis + onBasis.y * orthogonal;
    }

    public static Vector2 OnMagnitude(Vector2 toModify, Func<float, float> modification)
        => toModify.normalized * modification(toModify.magnitude);
}