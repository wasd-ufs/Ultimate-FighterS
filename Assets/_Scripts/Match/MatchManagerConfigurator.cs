using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManagerConfigurator : MatchConfigurator
{
    [SerializeField] public GameObject ManagerPrefab;

    public override void Configure()
    {
        MatchConfiguration.ManagerPrefab = ManagerPrefab;
    }
}
