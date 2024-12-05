using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    float minimumSize = 0.1f;
    public GameObject shootPoint;

    private float DetermineSize()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(shootPoint.transform.position, Vector2.left);
        float size = Mathf.Min(hit.distance, minimumSize);
        return size;
    }

    private void FixedUpdate()
    {
        transform.localPosition = new Vector3(DetermineSize()/2, 1, 1);
        transform.localScale = new Vector3(DetermineSize(), 1, 1);
    }
}
