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
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
