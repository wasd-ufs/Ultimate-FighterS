using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStageConfigurator : MatchConfigurator
{
    [SerializeField] public GameObject scene;

    public override void Configure()
    {
        MatchConfiguration.ScenePrefab = scene;
    }
}