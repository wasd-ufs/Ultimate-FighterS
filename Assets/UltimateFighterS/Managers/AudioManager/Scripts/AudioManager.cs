using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;

/// <summary>
/// Responsavel por gerenciar os audios do jogo
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance;

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("BackGroundMusic")]
    [SerializeField] private AudioSource _audioSourceBg;
    [SerializeField] private List<SettingBaseAudio> _settingBaseAudioList;
    [SerializeField] private List<SceneAudioData> _sceneAudioDataList;
    [SerializeField] private List<SceneAudioData> _scenePhaseAudioDataList;
    private bool _isPaused;
    private bool _isPhase = false;

    [Header("AudioEffects")]
    [SerializeField] private AudioSource _audioSourceExf;

    [Header("CrossFade Settings")]
    [SerializeField] private float _fadeDuration = 1f;
    private Coroutine _coroutine;
    

    void Awake() 
    {
        if (audioManagerInstance == null){ 
            audioManagerInstance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DetectedSceneOrPhase();
        PlayRadomBackGroudMusic();
    }

    void Update()
    {
        if (!_audioSourceBg.isPlaying && _settingBaseAudioList != null && !_isPaused && !_audioSourceBg.loop)
        {
            PlayRadomBackGroudMusic();
        }
    }

    /// <summary>
    /// Callback para quando uma nova cena eh carregada
    /// </summary>
    /// <param name="scene">Cena atual</param>
    /// <param name="mode">Modo da cena atual</param>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DetectedSceneOrPhase();
        PlayRadomBackGroudMusic();
    }

    /// <summary>
    /// Responsavel por definir a fase atual e muda as musicas
    /// </summary>
    /// <param name="phaseName">Nome da fase atual</param>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    public void SetCurrentPhase(string phaseName)
    {
        _isPhase = true;
        UpdateMusicListScene(phaseName, _isPhase);
        PlayRadomBackGroudMusic();
    }

    /// <summary>
    /// Responsavel por definir se é uma cena normal ou uma fase
    /// </summary>
    /// <return>void</return>
    /// <author>Wallisson de jesus</author>
    private void DetectedSceneOrPhase()
    {
        _isPhase = false;
        string sceneName = SceneManager.GetActiveScene().name;
        UpdateMusicListScene(sceneName, _isPhase);
    }

    /// <summary>
    /// Atualiza a lista de musicas do background para a configuracao de musicas da cena atual
    /// </summary>
    /// <param name="sceneName">Nome da cena atual</param>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    public void UpdateMusicListScene(string sceneName,bool isPhase)
    {
        SceneAudioData audioData;

        audioData = !isPhase ? _sceneAudioDataList.Find(d => d.SceneName == sceneName)
                             : _scenePhaseAudioDataList.Find(d => d.SceneName == sceneName);

        _settingBaseAudioList = audioData != null ? audioData.BackGroundClipList : null;
    }

    /// <summary>
    /// Escolhe uma musica aleatorio da cena atual
    /// </summary>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    private void PlayRadomBackGroudMusic()
    {
        if (_settingBaseAudioList == null || _settingBaseAudioList.Count == 0){ return; }

        SettingBaseAudio setting = _settingBaseAudioList[UnityEngine.Random.Range(0, _settingBaseAudioList.Count)];
       if (_coroutine != null)
       {
            StopCoroutine(_coroutine);
       }
        _coroutine = StartCoroutine(FadeMusic(setting));
    }

    /// <summary>
    ///  Toca uma musica aleatoria da cena, fazendo o fade entre as musicas
    /// </summary>
    /// <param name="clip">Audio que vai ser tocado</param>
    /// <returns>null</returns>
    /// <author>Wallisson de jesus</author>
    private IEnumerator FadeMusic(SettingBaseAudio settingBaseAudio)
    {
        if (_audioSourceBg.isPlaying)
        {
            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                _audioSourceBg.volume = Mathf.Lerp(1f, 0f, t / _fadeDuration);
                yield return null;
            }
        }

        float decibel = Mathf.Log10(Mathf.Clamp(settingBaseAudio.Volume, 0.0001f, 1f)) * 20;

        _audioSourceBg.clip = settingBaseAudio.Clip;
        _audioSourceBg.loop = settingBaseAudio.IsLoop;
        _audioMixer.SetFloat("MusicVolume", decibel);
        _audioMixer.SetFloat("MusicPitch", settingBaseAudio.Pitch);
        _audioSourceBg.Play();


        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            _audioSourceBg.volume = Mathf.Lerp(0f, 1f, t / _fadeDuration);
            yield return null;
        }

        _audioSourceBg.volume = 1f;
    }

    /// <summary>
    /// Executa uma audioClip especifico
    /// </summary>
    /// <param name="clip">O audioclip que vai ser tocado</param>
    /// <param name="volume">O volume do audioclip</param>
    /// <param name="picth">A velocidade do audioclip </param>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    public void PlayAudio(AudioClip clip,float volume,float picth) 
    {  
        _audioSourceExf.pitch = picth;
        _audioSourceExf.PlayOneShot(clip,volume);
    }

    /// <summary>
    /// Inicia as musicas de backGround
    /// </summary>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    public void StartBackGroundMusic()
    {
        _audioSourceBg.Play();
        _isPaused = false;
    }

    /// <summary>
    /// Para as musicas de backGround
    /// </summary>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    public void StopBackGroundMusic()
    {
        _audioSourceBg.Pause();
        _isPaused = true;
    }


}

