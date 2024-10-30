using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingDirection : MonoBehaviour
{
    public Transform player;

    public bool facingRight;

    void Start()
    {
        if (!facingRight)
        {
            Vector2 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
            facingRight = true;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;

        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void DirectionFace(Vector2 direction)
    {
        if (facingRight == false && direction.x > 0)
        {
            Flip();

        }else if (facingRight == true && direction.x < 0)
        {
            Flip();
        }
    }
}
