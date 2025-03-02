using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsavel por armazenar os dados de audio da cena
/// </summary>
[CreateAssetMenu(fileName ="Data",menuName ="Audio/Scene Data Audio BackGround")]
public class SceneAudioData : ScriptableObject
{
    [SerializeField] private string _sceneName;
    [SerializeField] private List<SettingBaseAudio> _backGroundClipList;

    public string SceneName => _sceneName;
    public List<SettingBaseAudio> BackGroundClipList => _backGroundClipList;
}
