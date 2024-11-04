using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAssembler : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(MatchConfiguration.ManagerPrefab);
        
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("StartPosition");

        int i = 0;
        foreach (GameObject player in MatchConfiguration.PlayersPrefabs)
        {
            var currentPlayer = Instantiate(player, spawnPoints[i].transform);
            var idComponent = currentPlayer.GetComponent<IdComponent>();
            
            if (idComponent is null)
                continue;
            
            idComponent.id = i;
            i++;
        }
    }
}
