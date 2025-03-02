using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsavel por armazenar os dados de audio da cena
/// </summary>
[CreateAssetMenu(fileName ="Data",menuName ="Audio/Scene Data Audio BackGround")]
public class SceneAudioData : ScriptableObject
{
    public string sceneName;
    public List<SettingBaseAudio> backgroundClipList;
}
