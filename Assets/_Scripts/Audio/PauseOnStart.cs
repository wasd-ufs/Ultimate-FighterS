using System;
using UnityEngine;

public class PauseOnStart : MonoBehaviour
{
    private AudioManager audioManager;

    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Pause();
    }

    public void Pause()
    {
        audioManager.Pause();
    }
}