using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class DefaultMatchAssembler : MonoBehaviour
{
    [ReadOnly] private const int RESULT_SCENE_INDEX = 4;
    [ReadOnly] private const string SPAWN_POINT_TAG = "SpawnPoint";

    private void Start()
    {
        MatchManager.OnMatchEnding.AddListener(_ => SceneChangerController.FadeOut(RESULT_SCENE_INDEX));

        Instantiate(MatchConfiguration.ScenePrefab);

        MatchManager.StartMatch();

        bool success = AddPlayers();
        if (!success)
            MatchManager.EndMatch();

        MatchManager.ForEachPlayer(player => player.Spawn());
        SceneChangerController.FadeIn();
    }

    private bool AddPlayers()
    {
        List<Transform> spawns = GetSpawnPoints();
        if (spawns.Count < MatchConfiguration.Characters.Count)
        {
            Debug.LogError(
                $"Not enough spawn points for all players! Point Count = {spawns.Count}. Player Count = {MatchConfiguration.Characters.Count}");
            return false;
        }

        int spawnPointIndex = 0;
        foreach ((int port, Character character) in MatchConfiguration.Characters)
        {
            Transform spawnPoint = spawns[spawnPointIndex];
            MatchManager.AddPlayer(port, MatchConfiguration.PlayerInputTypes[port], character, spawnPoint);

            spawnPointIndex++;
        }

        return true;
    }

    private List<Transform> GetSpawnPoints()
    {
        return GameObject.FindGameObjectsWithTag(SPAWN_POINT_TAG).ToList()
            .ConvertAll(spawnPoint => spawnPoint.transform);
    }
}