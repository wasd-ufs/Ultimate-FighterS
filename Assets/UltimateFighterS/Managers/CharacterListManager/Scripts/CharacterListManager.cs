using UnityEngine;
using UnityEngine.UI;

public class CharacterListManager : MonoBehaviour
{
    [SerializeField] private Image currentImage;
    [SerializeField] private Sprite[] characters;
    [SerializeField] public int currentIndex;


    public void ShowNextImage()
    {
        currentIndex = (currentIndex + 1) % characters.Length;
        UpdateImage();
    }

    // Method to show the previous image
    public void ShowPrevImage()
    {
        currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
        UpdateImage();
    }

    // Method to update the displayed image
    private void UpdateImage()
    {
        currentImage.sprite = characters[currentIndex];
    }
}