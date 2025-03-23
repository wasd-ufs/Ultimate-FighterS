using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class DefaultMatchAssembler : MonoBehaviour
{
    [ReadOnly] private const int ResultSceneIndex = 4;
    [ReadOnly] private const string SpawnPointTag = "SpawnPoint";

    private void Start()
    {
        MatchManager.OnMatchEnding.AddListener(_ => SceneChangerController.FadeOut(ResultSceneIndex));
        
        Instantiate(MatchConfiguration.ScenePrefab);
        
        MatchManager.StartMatch();
        
        if (!CanAddPlayers())
            MatchManager.EndMatch();
        else
            AddPlayers();
        
        MatchManager.ForEachPlayer(player => player.Spawn());
        SceneChangerController.FadeIn();
    }

    private bool CanAddPlayers()
    {
        List<Transform> spawns = GetSpawnPoints();
        if (spawns.Count < MatchConfiguration.Characters.Count)
        {
            Debug.LogError(
                $"Not enough spawn points for all players! Point Count = {spawns.Count}. Player Count = {MatchConfiguration.Characters.Count}");
            return false;
        }
        return true;
    }
    private void AddPlayers()
    {
        List<Transform> spawns = GetSpawnPoints();
        int spawnPointIndex = 0;
        foreach ((int port, Character character) in MatchConfiguration.Characters)
        {
            Transform spawnPoint = spawns[spawnPointIndex];
            MatchManager.AddPlayer(port, MatchConfiguration.PlayerInputTypes[port], character, spawnPoint);
            
            spawnPointIndex++;
        }
    }
    
    private List<Transform> GetSpawnPoints() => GameObject.FindGameObjectsWithTag(SpawnPointTag).ToList()
        .ConvertAll(spawnPoint => spawnPoint.transform);
}