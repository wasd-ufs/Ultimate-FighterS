using UnityEngine;

public class MusicPhase1_Manager : MonoBehaviour
{
    [Header("---Audio Sorce---")] [SerializeField]
    private AudioSource musicSource;

    [Header("---Audio Clip---")] [SerializeField]
    private AudioClip musicClip;

    private void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}