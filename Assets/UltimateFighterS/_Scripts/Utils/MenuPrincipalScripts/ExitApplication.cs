using UnityEditor;
using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    public void Closeaplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}