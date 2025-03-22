using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _skin;
    public void Hide()
    {
        _skin.SetActive(false);
    }
}
