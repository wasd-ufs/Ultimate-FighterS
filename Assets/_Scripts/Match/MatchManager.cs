using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class MatchManager
{
    public static readonly UnityEvent OnMatchStarting = new();
    public static readonly UnityEvent<List<ActivePlayer>> OnMatchEnding = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerEntering = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerExiting = new();
    
    private static readonly Dictionary<int, ActivePlayer> activePlayers = new();
    private static bool isMatchRunning;

    public static void StartMatch()
    {
        if (isMatchRunning)
            EndMatch();
        
        isMatchRunning = true;
        OnMatchStarting.Invoke();
    }

    public static void EndMatch()
    {
        if (!isMatchRunning)
            return;
        
        isMatchRunning = false;
        
        foreach (var players in activePlayers.Values)
        {
            players.OnPlayerSpawned.RemoveAllListeners();
            players.OnPlayerKilled.RemoveAllListeners();
        }
        
        OnMatchEnding.Invoke(activePlayers.Values.ToList());
        
        RemoveAllPlayers();
        
        OnMatchStarting.RemoveAllListeners();
        OnMatchEnding.RemoveAllListeners();
        OnPlayerEntering.RemoveAllListeners();
        OnPlayerExiting.RemoveAllListeners();
    }
    
    public static void AddPlayer(int port, InputType input, Character character, Transform spawnPoint)
    {
        var player = new ActivePlayer(port, input, character, spawnPoint);
        activePlayers[port] = player;
        OnPlayerEntering.Invoke(player);
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
        if (!activePlayers.TryGetValue(port, out var player))
            return;
        
        OnPlayerExiting.Invoke(activePlayers[port]);
        
        player.OnPlayerSpawned.RemoveAllListeners();
        player.OnPlayerKilled.RemoveAllListeners();
        player.Kill();
        activePlayers.Remove(port);
    }
    
    public static void RemoveAllPlayers()
    {
        var ports = activePlayers.Keys.ToArray();
        foreach (var t in ports)
            RemovePlayer(t);
    }

    public static void ForEachPlayer(Action<ActivePlayer> action)
    {
        foreach (var player in activePlayers.Values)
            action(player);
    }
    
    public static List<ActivePlayer> GetActivePlayers() => activePlayers.Values.ToList();

    public static ActivePlayer GetPlayer(GameObject inGameObject)
    {
        var idComponent = inGameObject.GetComponent<IdComponent>();
        if (idComponent is null)
            return null;
        
        return GetPlayer(idComponent.id);
    }
    
    public static ActivePlayer GetPlayer(int port) => activePlayers.GetValueOrDefault(port);
}
