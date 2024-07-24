using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSystemCharacter2D : MonoBehaviour
{

    public abstract Vector2 GetDerection();


    public abstract bool isJumpKeyDown ();
    

    public abstract bool isJumpKeyPressed ();

    
    public abstract bool isAtackKeyDown();

        
    public abstract bool isAtackKeyPressed();

}
