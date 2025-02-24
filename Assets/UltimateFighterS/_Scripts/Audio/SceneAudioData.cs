using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla os dados da scene como seu nome e  os audios que podem ser tocados
/// </summary>
[CreateAssetMenu(fileName ="Data",menuName ="Audio/Scene Audio Data")]
public class SceneAudioData : ScriptableObject
{
    public string sceneName;
    public AudioClip[] backgroundClips;
}
