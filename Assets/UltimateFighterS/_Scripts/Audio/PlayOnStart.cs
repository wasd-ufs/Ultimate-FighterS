using UnityEngine;

public class PlayOnStart : MonoBehaviour
{
    [SerializeField] private string clipName = "";
    private AudioManager _audioManager;

    public void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        Play();
    }

    public void Play()
    {
        //audioManager.PlayAudio(clipName);
    }
}