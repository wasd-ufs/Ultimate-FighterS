using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneChangerController : MonoBehaviour
{
    private static readonly int FadeInTrigger = Animator.StringToHash("in");
    private static readonly int FadeOutTrigger = Animator.StringToHash("out");
    private static Animator _animator;
    private static int _nextScene;

    private static GameObject _sceneChanger;

    public void Awake()
    {
        if (_sceneChanger != null)
        {
            Destroy(gameObject);
            return;
        }

        _sceneChanger = gameObject;
        DontDestroyOnLoad(gameObject);

        _animator = GetComponent<Animator>();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(_nextScene);
    }

    public static void FadeIn()
    {
        _animator.SetTrigger(FadeInTrigger);
        _animator.ResetTrigger(FadeOutTrigger);
    }

    public static void FadeOut(int sceneIndex)
    {
        _nextScene = sceneIndex;

        _animator.ResetTrigger(FadeInTrigger);
        _animator.SetTrigger(FadeOutTrigger);
    }
}