using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsavel por amazenar a configuracoes basicas dos audios
/// </summary>
[CreateAssetMenu(fileName = "SettingsBaseAudio", menuName = "Audio/SettingAudio")]
public class SettingBaseAudio : ScriptableObject
{
    public AudioClip[] clips;
    public float volume;
    public float pitch;
    public bool isLoop;
}
