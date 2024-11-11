using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTransitioner : MonoBehaviour
{
    const int MatchSceneIndex = 1;
     
    public void LoadStageScene()
    {
        if (!CanRun())
            return;
        
        SceneManager.LoadScene(MatchSceneIndex);
    }
    
    public bool CanRun() => MatchConfiguration.PlayersPrefabs.Count >= 2
        && MatchConfiguration.GameModePrefab is not null
        && MatchConfiguration.ScenePrefab is not null;
}
