using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTransitioner : MonoBehaviour
{
    public void LoadStageScene()
    {
        SceneManager.LoadScene(MatchConfiguration.Scene);
    }
}
