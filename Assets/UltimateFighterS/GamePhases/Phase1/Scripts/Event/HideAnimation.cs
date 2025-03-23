using UnityEngine;

public class HideAnimation : MonoBehaviour
{
    public GameObject skin;

    public void Hide()
    {
        skin.SetActive(false);
    }
}