using UnityEngine;
using UnityEngine.Events;

public class OnFloorBehaviour : CharacterState
{
    [SerializeField] public UnityEvent<Vector2> onFloorEnter;
    [SerializeField] public UnityEvent<Vector2> onFloorExit;

    public override void OnFloorEnter(Vector2 normal)
    {
        onFloorEnter.Invoke(normal);
    }

    public override void OnFloorExit(Vector2 normal)
    {
        onFloorExit.Invoke(normal);
    }
}