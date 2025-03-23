using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Toca um audio, com variações aleatórias no volume e tom, ao entrar no estado 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayAudioBehaviour : CharacterState
{
    [FormerlySerializedAs("pitchRandomRadius")] [SerializeField] private float _pitchRandomRadius = 0.1f;
    [FormerlySerializedAs("volumeRandomRadius")] [SerializeField] private float _volumeRandomRadius = 0.1f;

    private AudioSource _audioSource;
    private float _pitch;

    private float _volume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _volume = _audioSource.volume;
        _pitch = _audioSource.pitch;
    }

    public override void Enter()
    {
        PlayAudio();
    }

    /// <summary>
    /// Toca o audio com tom e volume aleatórios a cada execução.
    /// </summary>
    private void PlayAudio()
    {
        _audioSource.pitch = _pitch + Random.Range(-_pitchRandomRadius, _pitchRandomRadius);
        _audioSource.volume = _volume + Random.Range(-_volumeRandomRadius, _volumeRandomRadius);

        _audioSource.Play();
    }
}