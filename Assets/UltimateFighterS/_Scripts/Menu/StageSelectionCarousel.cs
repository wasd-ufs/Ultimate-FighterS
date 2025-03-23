using UnityEngine;

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
        Stage previousStage = StageRegistry.Stages[PreviousIndex];
        Stage selectedStage = StageRegistry.Stages[CurrentIndex];
        Stage nextStage = StageRegistry.Stages[NextIndex];

        WriteToMatchConfiguration(selectedStage);
        UpdateUI(previousStage, selectedStage, nextStage);
    }

    private int IndexFromMatchConfiguration()
    {
        return StageRegistry.Stages
            .FindIndex(stage => stage.prefab == MatchConfiguration.ScenePrefab);
    }

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

    protected override int GetItemCount()
    {
        return StageRegistry.StageCount;
    }
}