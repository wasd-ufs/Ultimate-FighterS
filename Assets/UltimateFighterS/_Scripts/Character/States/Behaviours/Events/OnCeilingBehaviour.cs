using UnityEngine;
using UnityEngine.Events;

public class OnCeilingBehaviour : CharacterState
{
    [SerializeField] public UnityEvent<Vector2> onCeilingEnter;
    [SerializeField] public UnityEvent<Vector2> onCeilingExit;

    public override void OnCeilingEnter(Vector2 normal)
    {
        onCeilingEnter.Invoke(normal);
    }

    public override void OnCeilingExit(Vector2 normal)
    {
        onCeilingExit.Invoke(normal);
    }
}