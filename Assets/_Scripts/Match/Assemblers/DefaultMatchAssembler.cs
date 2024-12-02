using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultMatchAssembler : MonoBehaviour
{
    [ReadOnly] private const int ResultSceneIndex = 0;
    [ReadOnly] private const string SpawnPointTag = "SpawnPoint";

    private void Start()
    {
        MatchManager.OnMatchEnding.AddListener(_ => SceneManager.LoadScene(ResultSceneIndex));
        
        Instantiate(MatchConfiguration.ScenePrefab);
        
        MatchManager.StartMatch();
        
        var success = AddPlayers();
        if (!success)
            MatchManager.EndMatch();
        
        MatchManager.ForEachPlayer(player => player.Spawn());
    }

    private bool AddPlayers()
    {
        var spawns = GetSpawnPoints();
        if (spawns.Count < MatchConfiguration.PlayersPrefabs.Count)
        {
            Debug.LogError($"Not enough spawn points for all players! Point Count = {spawns.Count}. Player Count = {MatchConfiguration.PlayersPrefabs.Count}");
            return false;
        }

        int spawnPointIndex = 0;
        foreach (var (port, player) in MatchConfiguration.PlayersPrefabs)
        {
            var spawnPoint = spawns[spawnPointIndex];
            MatchManager.AddPlayer(port, MatchConfiguration.PlayerInputTypes[port], player, spawnPoint);
            
            spawnPointIndex++;
        }

        return true;
    }
    
    private List<Transform> GetSpawnPoints() => GameObject.FindGameObjectsWithTag(SpawnPointTag).ToList()
        .ConvertAll(spawnPoint => spawnPoint.transform);
}