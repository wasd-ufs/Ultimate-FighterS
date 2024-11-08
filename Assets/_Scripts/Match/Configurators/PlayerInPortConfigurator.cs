using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInPortConfigurator : MatchConfigurator
{
    [SerializeField] public int port;
    [SerializeField] public GameObject prefab;

    public override void Configure()
    {
        if (prefab == null)
        {
            MatchConfiguration.PlayersPrefabs.Remove(port);
            MatchConfiguration.PlayerInputTypes.Remove(port);
            return;
        }
        
        MatchConfiguration.PlayersPrefabs[port] = prefab;
        MatchConfiguration.PlayerInputTypes[port] =
            MatchConfiguration.PlayerInputTypes.GetValueOrDefault(port, InputType.Player);
    }
}
