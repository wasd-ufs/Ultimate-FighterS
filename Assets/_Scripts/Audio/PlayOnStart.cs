using System;
using UnityEngine;

public class PlayOnStart : MonoBehaviour
{
    [SerializeField] private string clipName = "";
    private AudioManager audioManager;

    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Play();
    }

    public void Play()
    {
        audioManager.Play(clipName);
    }
}