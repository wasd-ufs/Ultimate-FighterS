using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersPrefabsCommand : MatchInterface
{
    [SerializeField] public List<GameObject> PlayersPrefabs { get; set; }

    public void Configure()
    {
        MatchConfiguration.Instance.PlayersPrefabs = PlayersPrefabs;
    }
}
