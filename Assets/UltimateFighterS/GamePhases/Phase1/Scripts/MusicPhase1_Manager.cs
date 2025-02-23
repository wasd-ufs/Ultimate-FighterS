using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPhase1_Manager : MonoBehaviour
{

    [Header("---Audio Sorce---")]
    [SerializeField] private AudioSource musicSource;

    [Header("---Audio Clip---")]
    [SerializeField] private AudioClip musicClip;

    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

}
