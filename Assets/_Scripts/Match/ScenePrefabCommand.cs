using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePrefabCommand : MatchInterface
{
    [SerializeField]  public GameObject ScenePrefab { get; set; }

    public void Configure()
    {
        MatchConfiguration.Instance.ScenePrefab = ScenePrefab;
    }
}