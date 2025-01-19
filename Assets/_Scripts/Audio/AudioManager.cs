using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Faz o gerenciamento dos audios
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManagerInstance;

    [Header("BackGroundMusic")]
    [SerializeField] private AudioSource _audioSourceBg;
    [SerializeField] private AudioClip[] _currentclipBg;
    [SerializeField] private List<SceneAudioData> _sceneAudioDataList;

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

        //Executa toda vez que a cena eh carregada 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!_audioSourceBg.isPlaying && _currentclipBg != null)
        {
            PlayRadomBackGroudMusic();
        }
    }

    /// <summary>
    /// Callback para quando uma nova cena eh carregada
    /// </summary>
    /// <param name="scene">Cena atual</param>
    /// <param name="mode">Modo da cena atual</param>
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
    public void UpdateMusicListScene(string sceneName)
    {
        SceneAudioData data = _sceneAudioDataList.Find(d => d.sceneName == sceneName);
        if (data != null)
        {
            _currentclipBg = data.backgroundClips;
        }
        else
        {
            _currentclipBg = null;
        }
    }

    /// <summary>
    /// Escolhe uma musica aleatorio da cena atual
    /// </summary>
    /// <returns>void</returns>
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
    ///  Toca uma musica aleatoria do cena fazendo o fade entre as musicas
    /// </summary>
    /// <param name="clip">Aduio que vai ser tocado</param>
    /// <returns>null</returns>
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
    public void PlayAudio(AudioClip clip,float volume,float picth) 
    {  
        _audioSourceExf.pitch = picth;
        _audioSourceExf.PlayOneShot(clip,volume);
    }
}

