using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioWithAnimation : MonoBehaviour
{
    [SerializeField] private float pitchRandomRadius = 0.1f;
    [SerializeField] private float volumeRandomRadius = 0.1f;

    private AudioSource _audioSource;
    private float _pitch;

    private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _volume = _audioSource.volume;
        _pitch = _audioSource.pitch;
    }

    public void PlayAudio()
    {
        _audioSource.pitch = _pitch + Random.Range(-pitchRandomRadius, pitchRandomRadius);
        _audioSource.volume = _volume + Random.Range(-volumeRandomRadius, volumeRandomRadius);

        _audioSource.Play();
    }
}