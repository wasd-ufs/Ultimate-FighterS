using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCommand : MatchInterface
{
    [SerializeField]  public string SceneName { get; set; }

    public void Configure()
    {
        SceneManager.LoadScene(SceneName);
    }
}
