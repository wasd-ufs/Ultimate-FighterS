using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameModePrefabConfigurator : MatchConfigurator
{
    [SerializeField] public GameObject gameModePrefab;

    public override void Configure()
    {
        MatchConfiguration.GameModePrefab = gameModePrefab;
    }
}
