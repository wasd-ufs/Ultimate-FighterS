using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPrefabCommand : MatchInterface
{
    [SerializeField] public GameObject ManagerPrefab { get; set; }

    public void Configure()
    {
        MatchConfiguration.Instance.ManagerPrefab = ManagerPrefab;
    }
}
