using UnityEngine;

public abstract class InputSystem : MonoBehaviour
{
    public abstract Vector2 GetDirection();

    public abstract bool IsSpecialBeingHeld();

    public abstract bool IsSpecialJustPressed();

    public abstract bool IsAttackBeingHeld();

    public abstract bool IsAttackJustPressed();

    public InputSystemMemento GetMemento()
    {
        return new InputSystemMemento(this);
    }
}