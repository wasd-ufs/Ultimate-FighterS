using UnityEngine;

public class FadeInOnStart : MonoBehaviour
{
    public void Start()
    {
        SceneChangerController.FadeIn();
    }
}