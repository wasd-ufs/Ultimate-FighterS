using System;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    private void Awake()
    {
        MatchManager.OnMatchStarting.AddListener(OnMatchStarting);
        MatchManager.OnMatchEnding.AddListener(OnMatchEnding);
        MatchManager.OnPlayerEntering.AddListener(OnPlayerEntering);
        MatchManager.OnPlayerExiting.AddListener(OnPlayerExiting);
        MatchManager.OnPlayerSpawned.AddListener(OnPlayerSpawned);
        MatchManager.OnPlayerKilled.AddListener(OnPlayerKilled);
    }

    protected virtual void OnMatchStarting() {}
    protected virtual void OnMatchEnding() {}
    protected virtual void OnPlayerEntering(ActivePlayer player) {}
    protected virtual void OnPlayerExiting(ActivePlayer player) {}
    protected virtual void OnPlayerSpawned(ActivePlayer player, GameObject obj) {}
    protected virtual void OnPlayerKilled(ActivePlayer player, GameObject obj) {}
}