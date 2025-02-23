using UnityEngine;
using UnityEngine.Events;

public class OnWallBehaviour : CharacterState
{
    [SerializeField] public UnityEvent<Vector2> onWallEnter;
    [SerializeField] public UnityEvent<Vector2> onWallExit;

    public override void OnLeftWallEnter(Vector2 normal)
    {
        onWallEnter.Invoke(normal);
    }
    
    public override void OnRightWallEnter(Vector2 normal)
    {
        onWallEnter.Invoke(normal);
    }

    public override void OnLeftWallExit(Vector2 normal)
    {
        onWallExit.Invoke(normal);
    }
    
    public override void OnRightWallExit(Vector2 normal)
    {
        onWallExit.Invoke(normal);
    }
}