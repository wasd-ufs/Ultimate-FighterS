using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Train : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetDefaultPosition();
    }               
            
    void Start()
    {
            
    }

    public void SetVelocity(float velocity)
    {
        rb.velocity = new Vector2(velocity,0);
    }

    public void SetPosition()
    {
        this.transform.position = new Vector3(-19, -3.6f, 0);
    }

    public void SetDefaultPosition()
    {
        this.transform.position = new Vector3(0, -10, transform.position.z);            
    }
}
