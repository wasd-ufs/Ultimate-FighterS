using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Responsavel por gerenciar os audios do jogo
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance;

    [Header("BackGroundMusic")]
    [SerializeField] private AudioSource _audioSourceBg;
    [SerializeField] private AudioClip[] _currentclipBg;
    [SerializeField] private List<SceneAudioData> _sceneAudioDataList;
    private bool _isPaused;

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
        UpdateMusicListScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!_audioSourceBg.isPlaying && _currentclipBg != null && !_isPaused)
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
        UpdateMusicListScene(scene.name);
        PlayRadomBackGroudMusic();
    }

    /// <summary>
    /// Atualiza a lista de musicas do background para a configuracao de musicas da cena atual
    /// </summary>
    /// <param name="sceneName">Nome da cena atual</param>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    public void UpdateMusicListScene(string sceneName)
    {
        SceneAudioData data = _sceneAudioDataList.Find(d => d.sceneName == sceneName);
        if (data != null)
        {
            _currentclipBg = data.backgroundClips;
            return;
        }
        
        _currentclipBg = null;
    }

    /// <summary>
    /// Escolhe uma musica aleatorio da cena atual
    /// </summary>
    /// <returns>void</returns>
    /// <author>Wallisson de jesus</author>
    private void PlayRadomBackGroudMusic()
    {
        AudioClip clip = _currentclipBg[UnityEngine.Random.Range(0, _currentclipBg.Length)];
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(FadeMusic(clip));
    }

    /// <summary>
    ///  Toca uma musica aleatoria da cena, fazendo o fade entre as musicas
    /// </summary>
    /// <param name="clip">Audio que vai ser tocado</param>
    /// <returns>null</returns>
    /// <author>Wallisson de jesus</author>
    private IEnumerator FadeMusic(AudioClip clip)
    {
        if (_audioSourceBg.isPlaying)
        {
            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                _audioSourceBg.volume = Mathf.Lerp(1f, 0f, t / _fadeDuration);
                yield return null;
            }
        }

        _audioSourceBg.clip = clip;
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

