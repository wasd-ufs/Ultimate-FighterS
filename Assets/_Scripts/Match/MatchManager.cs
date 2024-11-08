using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    const string SpawnPointTag = "SpawnPoint";
    const int EndMatchSceneIndex = 0;
    
    public static readonly UnityEvent OnMatchStarting = new();
    public static readonly UnityEvent OnMatchEnding = new();
    
    public static readonly UnityEvent<ActivePlayer, GameObject> OnPlayerSpawned = new();
    public static readonly UnityEvent<ActivePlayer, GameObject> OnPlayerKilled = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerEntering = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerExiting = new();
    
    private static readonly Dictionary<int, ActivePlayer> activePlayers = new();

    private void Start()
    {
        StartMatch();
    }

    private static void StartMatch()
    {
        RemoveAllPlayers();

        Instantiate(MatchConfiguration.ScenePrefab);
        
        Stack<Transform> availableSpawnPoints = new();
        foreach (var point in GameObject.FindGameObjectsWithTag(SpawnPointTag))
            availableSpawnPoints.Push(point.transform);
        
        if (availableSpawnPoints.Count < MatchConfiguration.PlayersPrefabs.Count)
        {
            Debug.LogError($"Not enough spawn points for all players! Point Count = {availableSpawnPoints.Count}. Player Count = {MatchConfiguration.PlayersPrefabs.Count}");
            return;
        }
        
        Instantiate(MatchConfiguration.GameModePrefab);
        OnMatchStarting.Invoke();
        
        foreach (var pair in MatchConfiguration.PlayersPrefabs)
            if (MatchConfiguration.PlayerInputTypes.TryGetValue(pair.Key, out var type))
                AddPlayer(pair.Key, type, pair.Value, availableSpawnPoints.Pop());
        
        SpawnAllPlayers();
    }
    
    private static void AddPlayer(int port, InputType input, GameObject prefab, Transform spawnPoint)
    {
        var player = new ActivePlayer(port, input, prefab, spawnPoint);
        activePlayers[port] = player;
        
        player.OnPlayerSpawned.AddListener(obj => OnPlayerSpawned.Invoke(player, obj));
        player.OnPlayerKilled.AddListener(obj => OnPlayerKilled.Invoke(player, obj));
        OnPlayerEntering.Invoke(player);
    }

    private static void RemoveAllPlayers()
    {
        var ports = activePlayers.Keys.ToArray();
        foreach (var t in ports)
            RemovePlayer(t);
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
        if (activePlayers.TryGetValue(port, out var player))
            player.Spawn();
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
        if (activePlayers.TryGetValue(port, out var player))
            player.Kill();
    }

    public static void EndMatch()
    {
        foreach (var players in activePlayers.Values)
        {
            players.OnPlayerSpawned.RemoveAllListeners();
            players.OnPlayerKilled.RemoveAllListeners();
        }
        
        OnMatchStarting.RemoveAllListeners();
        OnMatchEnding.RemoveAllListeners();
        OnPlayerEntering.RemoveAllListeners();
        OnPlayerExiting.RemoveAllListeners();
        OnPlayerSpawned.RemoveAllListeners();
        OnPlayerKilled.RemoveAllListeners();
        
        OnMatchEnding.Invoke();
        SceneManager.LoadScene(EndMatchSceneIndex);
    }

    public static List<GameObject> GetAlivePlayers()
    {
        var players = new List<GameObject>();
        foreach (var p in activePlayers.Values)
            if (p.InGameObject is not null)
                players.Add(p.InGameObject);
        
        return players;
    }
}

public class ActivePlayer
{
    public readonly UnityEvent<GameObject> OnPlayerSpawned = new();
    public readonly UnityEvent<GameObject> OnPlayerKilled = new();
    
    public int Port { get; private set; }
    public InputType Input { get; private set; }
    public GameObject InGameObject { get; private set; }
    public GameObject Prefab { get; private set; }
    public Transform SpawnPoint { get; private set; }

    public ActivePlayer(int port, InputType input, GameObject prefab, Transform spawnPoint)
    {
        Port = port;
        Input = input;
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

        var proxy = InGameObject.GetComponent<ProxyInputSystem>();
        if (proxy is null)
        {
            Debug.LogError("No ProxyInputSystem attached to Player Prefab");
            return;
        }

        proxy.input = Input switch
        {
            InputType.Player => InGameObject.AddComponent<PlayerInputSystem>(),
            InputType.NoInput => InGameObject.AddComponent<NoInputSystem>(),
            _ => throw new ArgumentOutOfRangeException()
        };

        var obj = InGameObject;
        OnPlayerSpawned.Invoke(obj);
    }

    public void Kill()
    {
        if (InGameObject is null)
            return;

        var obj = InGameObject;
        
        Object.Destroy(InGameObject);
        InGameObject = null;
        
        OnPlayerKilled.Invoke(obj);
        
    }
}