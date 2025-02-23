using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    [Header("Spring Propreties")]
    [SerializeField] private float elasticConstant;
    [SerializeField] private float dampingFactor;
    [SerializeField] private float maxStretch;

    [Header("Spring Animator")]
    [SerializeField] private Animator anim;


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Item"))
        {
            Rigidbody2D rig = other.gameObject.GetComponent<Rigidbody2D>();

            if (rig != null)
            {
                //Animation

                Vector2 stretch = (Vector2)transform.position - rig.position;
                float stretchMagnitude = stretch.magnitude;

                if (stretchMagnitude > maxStretch)
                {
                    stretch = stretch.normalized * maxStretch;
                }

                Vector2 elasticForce = -elasticConstant * stretch - dampingFactor * rig.velocity;

                rig.AddForce(elasticForce, ForceMode2D.Impulse);

            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //Animation
    }
}
