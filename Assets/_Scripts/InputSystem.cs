using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSystemCharacter2D : MonoBehaviour
{
    [SerializeField] protected KeyCode JumpKey;
    [SerializeField] protected KeyCode UpKey;
    [SerializeField] protected KeyCode RightKey;
    [SerializeField] protected KeyCode LeftKey;
    [SerializeField] protected KeyCode DownKey;
    [SerializeField] protected KeyCode AtackKey;


    public Vector2D GetDerection () {
        var left = Input.GetKey(LeftKey) ? 1f : 0f;
        var right = Input.GetKey(RightKey) ? 1f : 0f;
        var horizontal = right - left;
        var up = Input.GetKey(UpKey) ? 1f : 0f;
        var down = Input.GetKey(DownKey) ? 1f : 0f;
        var vertical = up - down;
        return newVector2(horizontal, vertical);
    }

    public bool isJumpKeyDown () => Input.GetKeyDown(JumpKey);
    

    public bool isJumpKeyPressed () => Input.GetKey(JumpKey); 
          

    public bool isAtackKeyDown () => Input.GetKeyDown(AtackKey);

        
    public bool isAtackKeyPressed () => Input.GetKey(AtackKey);

}
