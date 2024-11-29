using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAnimation : MonoBehaviour
{
    public GameObject skin;
    public void Hide()
    {
        skin.SetActive(false);
    }
}
