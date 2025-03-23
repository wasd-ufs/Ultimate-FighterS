using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Tipos de Superfície que podem ser checados
/// </summary>
public enum SurfaceType
{
    Floor,
    Wall,
    Ceiling,
    Any
}

/// <summary>
/// Eventos relacionados a superfície
/// </summary>
public enum SurfaceEventType
{
    Entering,
    Exiting
}

public class SurfaceTransitionBehaviour : CharacterState
{
    [FormerlySerializedAs("Surface")] [FormerlySerializedAs("Land")] [FormerlySerializedAs("land")] [SerializeField] private SurfaceType _surface;
    [FormerlySerializedAs("Event")] [FormerlySerializedAs("Type")] [FormerlySerializedAs("type")] [SerializeField] private SurfaceEventType _event;
    [FormerlySerializedAs("Next")] [FormerlySerializedAs("next")] [SerializeField] private CharacterState _next;

    public override void Enter()
    {
        if (EventHappened())
            Machine.TransitionTo(_next);
    }

    public override void StateFixedUpdate()
    {
        if (EventHappened())
            Machine.TransitionTo(_next);
    }

    /// <summary>
    /// Indica se o evento de superfície especificado aconteceu 
    /// </summary>
    /// <returns>se o evento de superfície aconteceu </returns>
    private bool EventHappened()
    {
        return (IsOnSurface() && _event == SurfaceEventType.Entering) || (!IsOnSurface() && _event == SurfaceEventType.Exiting);
    }

    /// <summary>
    /// Indica se o corpo está em contato com a superfície especificada
    /// </summary>
    /// <returns>Se o corpo está em contato com a superfície especificada</returns>
    public bool IsOnSurface() => _surface switch
    {
        SurfaceType.Floor => Body.IsOnFloor(),
        SurfaceType.Ceiling => Body.IsOnCeiling(), 
        SurfaceType.Wall => Body.IsOnLeftWall() || Body.IsOnRightWall(),
        _ => Body.IsOnFloor() || Body.IsOnCeiling() || Body.IsOnLeftWall() || Body.IsOnRightWall()
    };
}