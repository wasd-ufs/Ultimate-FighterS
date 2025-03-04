using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsavel por amazenar a configuracoes basicas do audio
/// </summary>
[CreateAssetMenu(fileName = "SettingsBaseAudio", menuName = "Audio/SettingAudio")]
public class SettingBaseAudio : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _volume;
    [SerializeField] private float _pitch;
    [SerializeField] private bool _isLoop;

    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public float Pitch => _pitch;
    public bool IsLoop => _isLoop;
}
