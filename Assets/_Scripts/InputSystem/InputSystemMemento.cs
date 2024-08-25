using System;
using UnityEngine;

[Serializable]
public class InputSystemMemento
{
    public Vector2 direction;
    
    public bool isAttackJustPressed;
    public bool isSpecialJustPressed;
    
    public bool isAttackBeingHeld;
    public bool isSpecialBeingHeld;

    public InputSystemMemento(InputSystem input)
    {
        direction = input.GetDirection();
        
        isAttackJustPressed = input.IsAttackJustPressed();
        isSpecialJustPressed = input.IsSpecialJustPressed();

        isAttackBeingHeld = input.isAttackBeingHeld();
        isSpecialBeingHeld = input.IsSpecialBeingHeld();
    }
}