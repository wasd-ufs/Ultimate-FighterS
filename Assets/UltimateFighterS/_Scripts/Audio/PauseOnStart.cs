using UnityEngine;

public class PauseOnStart : MonoBehaviour
{
    private AudioManager _audioManager;

    public void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        Pause();
    }

    public void Pause()
    {
        //audioManager.Pause();
    }
}