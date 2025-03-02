using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SettingsBaseAudio", menuName = "Audio/SettingAudio")]
public class SettingsBaseAudio : MonoBehaviour
{
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool isLoop;
}
