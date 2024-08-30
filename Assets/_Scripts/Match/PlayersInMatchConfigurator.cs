using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInMatchConfigurator : MatchConfigurator
{
    [SerializeField] public List<GameObject> PlayersPrefabs;

    public override void Configure()
    {
        MatchConfiguration.PlayersPrefabs = PlayersPrefabs;
    }
}
