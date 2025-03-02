using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsavel por amazenar a configuracoes basicas do audio
/// </summary>
[CreateAssetMenu(fileName = "SettingsBaseAudio", menuName = "Audio/SettingAudio")]
public class SettingBaseAudio : ScriptableObject
{
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool isLoop;
}
