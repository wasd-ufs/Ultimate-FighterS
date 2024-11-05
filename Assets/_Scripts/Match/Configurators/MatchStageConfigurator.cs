using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStageConfigurator : MatchConfigurator
{
    [SerializeField] public int scene;

    public override void Configure()
    {
        MatchConfiguration.Scene = scene;
    }
}