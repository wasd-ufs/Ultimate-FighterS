using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class StageSelectionCarousel : Carousel
{
    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
    }

    private void Start()
    {
        Select(IndexFromMatchConfiguration());
    }

    protected override void OnSelected(int index)
    {
        var selectedStage = StageRegistry.Stages[index];
        
        WriteToMatchConfiguration(selectedStage);
        UpdateUI(selectedStage);
    }

    private int IndexFromMatchConfiguration() => StageRegistry.Stages
        .FindIndex(stage => stage.prefab == MatchConfiguration.ScenePrefab);

    private void WriteToMatchConfiguration(Stage selectedStage)
    {
        MatchConfiguration.ScenePrefab = selectedStage.prefab;
    }

    private void UpdateUI(Stage selectedStage)
    {
        image.texture = selectedStage.icon;
    }

    protected override int GetItemCount() => StageRegistry.StageCount;
}
