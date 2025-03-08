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
        
        bool _success = AddPlayers();
        if (!_success)
            MatchManager.EndMatch();
        
        MatchManager.ForEachPlayer(player => player.Spawn());
        SceneChangerController.FadeIn();
    }

    private bool AddPlayers()
    {
        List<Transform> _spawns = GetSpawnPoints();
        if (_spawns.Count < MatchConfiguration.Characters.Count)
        {
            Debug.LogError($"Not enough spawn points for all players! Point Count = {_spawns.Count}. Player Count = {MatchConfiguration.Characters.Count}");
            return false;
        }

        int _spawnPointIndex = 0;
        foreach (var (_port, _character) in MatchConfiguration.Characters)
        {
            var _spawnPoint = _spawns[_spawnPointIndex];
            MatchManager.AddPlayer(_port, MatchConfiguration.PlayerInputTypes[_port], _character, _spawnPoint);
            
            _spawnPointIndex++;
        }

        return true;
    }
    
    private List<Transform> GetSpawnPoints() => GameObject.FindGameObjectsWithTag(SpawnPointTag).ToList()
        .ConvertAll(spawnPoint => spawnPoint.transform);
}