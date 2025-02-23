using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectionCarousel : Carousel
{
    [SerializeField] private StageDisplay previous;
    [SerializeField] private StageDisplay current;
    [SerializeField] private StageDisplay next;

    private void Start()
    {
        Select(IndexFromMatchConfiguration());
    }

    protected override void OnSelected(int index)
    {
        var previousStage = StageRegistry.Stages[PreviousIndex];
        var selectedStage = StageRegistry.Stages[CurrentIndex];
        var nextStage = StageRegistry.Stages[NextIndex];
        
        WriteToMatchConfiguration(selectedStage);
        UpdateUI(previousStage, selectedStage, nextStage);
    }

    private int IndexFromMatchConfiguration() => StageRegistry.Stages
        .FindIndex(stage => stage.prefab == MatchConfiguration.ScenePrefab);

    private void WriteToMatchConfiguration(Stage selectedStage)
    {
        MatchConfiguration.ScenePrefab = selectedStage.prefab;
    }

    private void UpdateUI(Stage previousStage, Stage selectedStage, Stage nextStage)
    {
        previous.SetStage(previousStage);
        current.SetStage(selectedStage);
        next.SetStage(nextStage);
    }

    protected override int GetItemCount() => StageRegistry.StageCount;
}
