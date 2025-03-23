using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
///     Faz o gerenciamento dos audios
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioManagerInstance;

    [FormerlySerializedAs("_audioSourceBg")] [Header("BackGroundMusic")] [SerializeField]
    private AudioSource audioSourceBg;

    [FormerlySerializedAs("_currentclipBg")] [SerializeField] private AudioClip[] currentclipBg;
    [FormerlySerializedAs("_sceneAudioDataList")] [SerializeField] private List<SceneAudioData> sceneAudioDataList;

    [FormerlySerializedAs("_audioSourceExf")] [Header("AudioEffects")] [SerializeField]
    private AudioSource audioSourceExf;

    [FormerlySerializedAs("_fadeDuration")] [Header("CrossFade Settings")] [SerializeField]
    private float fadeDuration = 1f;

    private Coroutine _coroutine;
    private bool _isPaused;


    private void Awake()
    {
        if (AudioManagerInstance == null)
        {
            AudioManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMusicListScene(SceneManager.GetActiveScene().name);

        //Executa toda vez que a cena eh carregada 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (!audioSourceBg.isPlaying && currentclipBg != null && !_isPaused) PlayRadomBackGroudMusic();
    }

    /// <summary>
    ///     Callback para quando uma nova cena eh carregada
    /// </summary>
    /// <param name="scene">Cena atual</param>
    /// <param name="mode">Modo da cena atual</param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicListScene(scene.name);
        PlayRadomBackGroudMusic();
    }

    /// <summary>
    ///     Atualiza a lista de musicas do background para a configuracao de musicas da cena atual
    /// </summary>
    /// <param name="sceneName">Nome da cena atual</param>
    /// <returns>void</returns>
    public void UpdateMusicListScene(string sceneName)
    {
        SceneAudioData data = sceneAudioDataList.Find(d => d.sceneName == sceneName);
        if (data != null)
            currentclipBg = data.backgroundClips;
        else
            currentclipBg = null;
    }

    /// <summary>
    ///     Escolhe uma musica aleatorio da cena atual
    /// </summary>
    /// <returns>void</returns>
    private void PlayRadomBackGroudMusic()
    {
        AudioClip clip = currentclipBg[Random.Range(0, currentclipBg.Length)];
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeMusic(clip));
    }

    /// <summary>
    ///     Toca uma musica aleatoria do cena fazendo o fade entre as musicas
    /// </summary>
    /// <param name="clip">Aduio que vai ser tocado</param>
    /// <returns>null</returns>
    private IEnumerator FadeMusic(AudioClip clip)
    {
        if (audioSourceBg.isPlaying)
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSourceBg.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }

        audioSourceBg.clip = clip;
        audioSourceBg.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSourceBg.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        audioSourceBg.volume = 1f;
    }

    /// <summary>
    ///     Executa uma audioClip especifico
    /// </summary>
    /// <param name="clip">O audioclip que vai ser tocado</param>
    /// <param name="volume">O volume do audioclip</param>
    /// <param name="picth">A velocidade do audioclip </param>
    /// <returns>void</returns>
    public void PlayAudio(AudioClip clip, float volume, float picth)
    {
        audioSourceExf.pitch = picth;
        audioSourceExf.PlayOneShot(clip, volume);
    }

    /// <summary>
    ///     Inicia as musicas de backGround
    /// </summary>
    /// <returns>void</returns>
    public void StartBackGroundMusic()
    {
        audioSourceBg.Play();
        _isPaused = false;
    }

    /// <summary>
    ///     Para as musicas de backGround
    /// </summary>
    /// <returns>void</returns>
    public void StopBackGroundMusic()
    {
        audioSourceBg.Pause();
        _isPaused = true;
    }
}