using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class MatchManager
{
    public static readonly UnityEvent OnMatchStarting = new();
    public static readonly UnityEvent<List<ActivePlayer>> OnMatchEnding = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerEntering = new();
    public static readonly UnityEvent<ActivePlayer> OnPlayerExiting = new();

    private static readonly Dictionary<int, ActivePlayer> ActivePlayers = new();
    private static bool _isMatchRunning;

    public static void StartMatch()
    {
        if (_isMatchRunning)
            EndMatch();

        _isMatchRunning = true;
        OnMatchStarting.Invoke();
    }

    public static void EndMatch()
    {
        if (!_isMatchRunning)
            return;

        _isMatchRunning = false;

        foreach (ActivePlayer players in ActivePlayers.Values)
        {
            players.OnPlayerSpawned.RemoveAllListeners();
            players.OnPlayerKilled.RemoveAllListeners();
        }

        OnMatchEnding.Invoke(ActivePlayers.Values.ToList());

        RemoveAllPlayers();

        OnMatchStarting.RemoveAllListeners();
        OnMatchEnding.RemoveAllListeners();
        OnPlayerEntering.RemoveAllListeners();
        OnPlayerExiting.RemoveAllListeners();
    }

    public static void AddPlayer(int port, InputType input, Character character, Transform spawnPoint)
    {
        ActivePlayer player = new(port, input, character, spawnPoint);
        ActivePlayers[port] = player;
        OnPlayerEntering.Invoke(player);
    }

    public static void RemovePlayer(GameObject player)
    {
        IdComponent idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;

        RemovePlayer(idComponent.id);
    }

    public static void RemovePlayer(int port)
    {
        if (!ActivePlayers.TryGetValue(port, out ActivePlayer player))
            return;

        OnPlayerExiting.Invoke(ActivePlayers[port]);

        player.OnPlayerSpawned.RemoveAllListeners();
        player.OnPlayerKilled.RemoveAllListeners();
        player.Kill();
        ActivePlayers.Remove(port);
    }

    public static void RemoveAllPlayers()
    {
        int[] ports = ActivePlayers.Keys.ToArray();
        foreach (int t in ports)
            RemovePlayer(t);
    }

    public static void ForEachPlayer(Action<ActivePlayer> action)
    {
        foreach (ActivePlayer player in ActivePlayers.Values)
            action(player);
    }

    public static List<ActivePlayer> GetActivePlayers()
    {
        return ActivePlayers.Values.ToList();
    }

    public static ActivePlayer GetPlayer(GameObject inGameObject)
    {
        IdComponent idComponent = inGameObject.GetComponent<IdComponent>();
        if (idComponent is null)
            return null;

        return GetPlayer(idComponent.id);
    }

    public static ActivePlayer GetPlayer(int port)
    {
        return ActivePlayers.GetValueOrDefault(port);
    }
}