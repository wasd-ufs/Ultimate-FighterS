using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputSystemCharacter2D : MonoBehaviour
{

    public abstract Vector2 GetDerection(InputAction.CallbackContext context);


    public abstract bool isJumpKeyDown (InputAction.CallbackContext context);
    

    public abstract bool isJumpKeyPressed (InputAction.CallbackContext context);

    
    public abstract bool isAtackKeyDown(InputAction.CallbackContext context);

        
    public abstract bool isAtackKeyPressed(InputAction.CallbackContext context);

}
