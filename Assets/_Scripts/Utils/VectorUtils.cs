using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VectorUtils
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
    
    public static Vector2 Avarage(List<Vector2> vectors) => vectors
        .Aggregate(Vector2.zero, (current, next) => current + next) / vectors.Count;

    public static Vector2 Orthogonal(Vector2 vector) => new Vector2(-vector.y, vector.x);

    public static Vector2 Closest(Vector2 vector, List<Vector2> comparisons) => comparisons
        .OrderByDescending(v => Vector2.Dot(v, vector) / v.sqrMagnitude)
        .FirstOrDefault();

    public static Vector2 Rotated(Vector2 vector, float angle)
    {
        var sin = Mathf.Sin(angle);
        var cos = Mathf.Cos(angle);
        
        return new Vector2(cos * vector.x + sin * vector.y, - sin * vector.x + cos * vector.y);
    }
    
    public static float Wedge(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;
    
    public static Vector2 Reflected(Vector2 vector, Vector2 normal) => vector - (2f * Vector2.Dot(vector, normal) * normal);
}