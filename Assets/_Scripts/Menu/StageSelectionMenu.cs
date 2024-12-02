using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectionMenu : MonoBehaviour
{
    public List<Stage> stages = new();
    private int currentStageIndex;
    private Stage currentStage => stages[currentStageIndex];
    
    public RawImage sprite;
    public InputSystem input;

    private int lastDirection = 0;

    public void Start()
    {
        ReadFromMatchConfiguration();
        
        WriteToMatchConfiguration();
        UpdateUI();
    }

    public void Update()
    {
        var input = Mathf.Clamp(
            this.input.GetDirection().x, 
            -1, 
            1
        );
        
        var direction = input > 0 ? 1 : input < 0 ? -1 : 0;
        
        if (direction != lastDirection)
            Select(currentStageIndex + direction + stages.Count);
        
        lastDirection = direction;
    }

    public void Select(int index)
    {
        currentStageIndex = index % stages.Count;
        
        WriteToMatchConfiguration();
        UpdateUI();
    }

    public void UpdateUI()
    {
        sprite.texture = currentStage.image;
    }

    private void WriteToMatchConfiguration()
    {
        MatchConfiguration.ScenePrefab = stages[currentStageIndex].stagePrefab;
    }

    private void ReadFromMatchConfiguration()
    {
        currentStageIndex = MatchConfiguration.ScenePrefab == null ? 0 :
            stages.FindIndex(x => x.stagePrefab == MatchConfiguration.ScenePrefab);
    }
}

[Serializable]
public class Stage
{
    public Texture image;
    public GameObject stagePrefab;
}