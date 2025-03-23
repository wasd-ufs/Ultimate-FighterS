using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class StageDisplay : MonoBehaviour
{
    private RawImage _image;

    public void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    public void SetStage(Stage stage)
    {
        _image.texture = stage.icon;
    }
}