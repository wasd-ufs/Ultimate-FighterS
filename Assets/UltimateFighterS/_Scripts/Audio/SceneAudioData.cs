using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla os dados da cena como seu nome e audios que podem ser tocados
/// </summary>
[CreateAssetMenu(fileName ="Data",menuName ="Audio/Scene Audio Data")]
public class SceneAudioData : ScriptableObject
{
    public string sceneName;
    public AudioClip[] backgroundClips;
}
