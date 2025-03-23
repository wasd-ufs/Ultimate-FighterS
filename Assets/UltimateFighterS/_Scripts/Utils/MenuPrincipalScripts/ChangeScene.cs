using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private int scene;

    public void Changescene()
    {
        SceneManager.LoadScene(scene);
    }
}