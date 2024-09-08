using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private float spritelength;
    private Vector2 startPos;
    public float speedParallaxEfx;

    void Start()
    {
        startPos = transform.position;
        spritelength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        //0 = Dont move & 1 = Move with cam
        Vector2 distance = cam.position * speedParallaxEfx;

        //Move
        transform.position = new Vector3(startPos.x + distance.x, startPos.y + distance.y,transform.position.z);
    }
}
