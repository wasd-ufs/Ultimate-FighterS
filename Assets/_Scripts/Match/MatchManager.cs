using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    const string SpawnPointTag = "SpawnPoint";
    const int EndMatchSceneIndex = 0;
    
    public static readonly UnityEvent<GameObject> OnPlayerRespawned = new();
    public static readonly UnityEvent<GameObject> OnPlayerEntering = new();
    public static readonly UnityEvent<GameObject> OnPlayerExiting = new();

    private static Dictionary<int, Transform> spawnPoints = new();
    private static Dictionary<int, GameObject> playerObjects = new();
    
    private void Awake()
    {
        FetchSpawnPoints();

        foreach (var players in MatchConfiguration.PlayersPrefabs)
            AddPlayer(players);

        Instantiate(MatchConfiguration.GamemodePrefab);
    }

    private static void FetchSpawnPoints()
    {
        spawnPoints.Clear();

        int id = 0;
        foreach (var spawnPoint in GameObject.FindGameObjectsWithTag(SpawnPointTag))
        {
            spawnPoints[id] = spawnPoint.transform;
            id++;
        }
        
        if (spawnPoints.Count < 4)
            Debug.Log($"Potentially insufficient spawn points. Count = {spawnPoints.Count}");
    }

    private static void AddPlayer(GameObject playerPrefab)
    {
        var player = Instantiate(playerPrefab, spawnPoints[playerObjects.Count % spawnPoints.Count]);
        
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;

        idComponent.id = playerObjects.Count;
        
        playerObjects[idComponent.id] = player;
        OnPlayerEntering.Invoke(player);
    }

    public static void RemovePlayer(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        playerObjects.Remove(idComponent.id);
        OnPlayerExiting.Invoke(player);
        
        Destroy(playerObjects[idComponent.id]);
    }
    
    public static void RespawnPlayer(GameObject player) {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        playerObjects[idComponent.id] = Instantiate(playerObjects[idComponent.id], spawnPoints[idComponent.id]);
        OnPlayerRespawned.Invoke(playerObjects[idComponent.id]);
    }
    
    public static void EndMatch()
    {
        SceneManager.LoadScene(EndMatchSceneIndex);
    }
}
