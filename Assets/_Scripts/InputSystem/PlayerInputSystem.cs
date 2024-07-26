using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : InputSystemCharacter2D
{

    public override Vector2 GetDerection(InputAction.CallbackContext context)
    { 
        Vector2 inputValue = context.ReadValue<Vector2>();
        return inputValue;
    }
    public override bool isJumpKeyDown(InputAction.CallbackContext context) 
    {
        bool state = false;
        if (context.performed) state = true;
        else if (context.canceled) state = false;

        return state;
    }

    public override bool isJumpKeyPressed(InputAction.CallbackContext context) 
    {
        return context.performed;
    }

    public override bool isAtackKeyDown(InputAction.CallbackContext context) 
    {
        bool state = false;
        if (context.performed) state = true;
        else if (context.canceled) state = false;

        return state;
    }

    public override bool isAtackKeyPressed(InputAction.CallbackContext context) 
    {
        return context.performed; 
    }

}
