using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class StageDisplay : MonoBehaviour
{
    private RawImage image;

    public void Awake()
    {
        image = GetComponent<RawImage>();
    }

    public void SetStage(Stage stage)
    {
        image.texture = stage.icon;
    }
}