using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioBehaviour : CharacterState
{
    [SerializeField] private float pitchRandomRadius = 0.1f;
    [SerializeField] private float volumeRandomRadius = 0.1f;
    
    private AudioSource audioSource;
    
    private float volume;
    private float pitch;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
        pitch = audioSource.pitch;
    }

    public override void Enter()
    {
        PlayAudio();
    }

    void PlayAudio()
    {
        audioSource.pitch = pitch + Random.Range(-pitchRandomRadius, pitchRandomRadius);
        audioSource.volume = volume + Random.Range(-volumeRandomRadius, volumeRandomRadius);
        
        audioSource.Play();
    }
}