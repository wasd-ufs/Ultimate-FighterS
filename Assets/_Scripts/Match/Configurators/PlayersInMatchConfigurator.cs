using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInMatchConfigurator : MatchConfigurator
{
    [SerializeField] public int port;
    [SerializeField] public GameObject prefab;

    public override void Configure()
    {
        MatchConfiguration.PlayersPrefabs[port] = prefab;
    }
}
