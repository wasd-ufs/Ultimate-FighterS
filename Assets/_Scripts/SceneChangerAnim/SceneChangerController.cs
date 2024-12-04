using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerController : MonoBehaviour
{
    public Animator animator;

    public void ActivateFadeOut()
    {
        animator.SetBool("Active", true);
    }

    public void ActivateFadeIn()
    {
        animator.SetBool("Active", false);
    }
}
