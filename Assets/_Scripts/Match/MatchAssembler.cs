using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAssembler : MonoBehaviour
{
    private GameObject currentPlayer;
    private GameObject[] spawnPoints;
    private int i = 0;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("StartPosition");
        Instantiate(MatchConfiguration.ManagerPrefab);

        foreach (GameObject player in MatchConfiguration.PlayersPrefabs)
        {
            currentPlayer = Instantiate(player, spawnPoints[i].transform);
            currentPlayer.TryGetComponent<IdComponent>(out IdComponent idComponent);
            idComponent.id = i;
            i++;
        }
    }
}
