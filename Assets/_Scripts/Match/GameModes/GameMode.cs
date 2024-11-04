using System;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    private void Awake()
    {
        MatchManager.OnPlayerEntering.AddListener(OnPlayerEntering);
        MatchManager.OnPlayerExiting.AddListener(OnPlayerExiting);
        MatchManager.OnPlayerRespawned.AddListener(OnPlayerRespawned);
        DeathComponent.OnPlayerDeath.AddListener(OnPlayerDeath);
    }

    protected virtual void OnPlayerEntering(GameObject player) {}
    protected virtual void OnPlayerExiting(GameObject player) {}
    protected virtual void OnPlayerRespawned(GameObject player) {}
    protected virtual void OnPlayerDeath(GameObject player) {}
}