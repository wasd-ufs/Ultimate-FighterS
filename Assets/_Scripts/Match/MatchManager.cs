using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    const string SpawnPointTag = "SpawnPoint";
    const int EndMatchSceneIndex = 0;
    
    public static readonly UnityEvent OnMatchStarting = new();
    public static readonly UnityEvent OnMatchEnding = new();
    
    public static readonly UnityEvent<ActivePlayer> OnPlayerSpawned = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerKilled = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerEntering = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerExiting = new();
    
    private static Dictionary<int, ActivePlayer> activePlayers = new();

    private void Start()
    {
        StartMatch();
    }

    private static void StartMatch()
    {
        RemoveAllPlayers();
        
        Stack<Transform> availableSpawnPoints = new();
        foreach (var point in GameObject.FindGameObjectsWithTag(SpawnPointTag))
            availableSpawnPoints.Push(point.transform);
        
        Instantiate(MatchConfiguration.GameModePrefab);
        OnMatchStarting.Invoke();
        
        foreach (var pair in MatchConfiguration.PlayersPrefabs)
            AddPlayer(pair.Key, pair.Value, availableSpawnPoints.Pop());
        
        SpawnAllPlayers();
    }
    
    private static void AddPlayer(int port, GameObject prefab, Transform spawnPoint)
    {
        var player = new ActivePlayer(port, prefab, spawnPoint);
        activePlayers[port] = player;
        
        player.OnPlayerSpawned.AddListener(() => OnPlayerSpawned.Invoke(player));
        player.OnPlayerKilled.AddListener(() => OnPlayerKilled.Invoke(player));
        OnPlayerEntering.Invoke(player);
    }

    private static void RemoveAllPlayers()
    {
        foreach (var player in activePlayers.Values)
            RemovePlayer(player.Port);
    }

    public static void RemovePlayer(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        RemovePlayer(idComponent.id);
    }

    public static void RemovePlayer(int port)
    {
        if (!activePlayers.ContainsKey(port))
            return;
        
        activePlayers[port].OnPlayerSpawned.RemoveAllListeners();
        activePlayers[port].OnPlayerKilled.RemoveAllListeners();
        activePlayers[port].Kill();
        
        OnPlayerExiting.Invoke(activePlayers[port]);
        activePlayers.Remove(port);
    }
    
    public static void RespawnPlayer(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        RespawnPlayer(idComponent.id);
    }

    public static void RespawnPlayer(int port)
    {
        KillPlayer(port);
        SpawnPlayer(port);
    }

    private static void SpawnAllPlayers()
    {
        foreach (var player in activePlayers.Values)
            player.Spawn();
    }
    
    public static void SpawnPlayer(int port)
    {
        if (activePlayers.ContainsKey(port))
            activePlayers[port].Spawn();
    }

    public static void KillPlayer(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        KillPlayer(idComponent.id);
    }

    public static void KillPlayer(int port)
    {
        if (activePlayers.ContainsKey(port))
            activePlayers[port].Kill();
    }

    public static void EndMatch()
    {
        foreach (var players in activePlayers.Values)
        {
            players.OnPlayerSpawned.RemoveAllListeners();
            players.OnPlayerKilled.RemoveAllListeners();
        }
        
        OnMatchEnding.Invoke();
        SceneManager.LoadScene(EndMatchSceneIndex);
    }
}

public class ActivePlayer
{
    public UnityEvent OnPlayerSpawned = new();
    public UnityEvent OnPlayerKilled = new();
    
    public int Port { get; private set; }
    public GameObject InGameObject { get; private set; }
    public GameObject Prefab { get; private set; }
    public Transform SpawnPoint { get; private set; }

    public ActivePlayer(int port, GameObject prefab, Transform spawnPoint)
    {
        Port = port;
        Prefab = prefab;
        SpawnPoint = spawnPoint;
        InGameObject = null;
    }

    public void Spawn()
    {
        if (InGameObject is not null)
            Object.Destroy(InGameObject);
        
        InGameObject = Object.Instantiate(Prefab, SpawnPoint);
        var idComponent = InGameObject.GetComponent<IdComponent>();
        if (idComponent is null)
        {
            Debug.LogError("No IdComponent attached to Player Prefab");
            return;
        }
        
        idComponent.id = Port;
        OnPlayerSpawned.Invoke();
    }

    public void Kill()
    {
        if (InGameObject is null)
            return;

        Object.Destroy(InGameObject);
        InGameObject = null;
        
        OnPlayerKilled.Invoke();
    }
}