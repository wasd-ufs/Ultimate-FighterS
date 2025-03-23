using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    private void Awake()
    {
        MatchManager.OnMatchStarting.AddListener(OnMatchStarting);
        MatchManager.OnMatchEnding.AddListener(OnMatchEnding);
        MatchManager.OnPlayerEntering.AddListener(OnPlayerEntering);
        MatchManager.OnPlayerExiting.AddListener(OnPlayerExiting);
    }

    protected virtual void OnMatchStarting()
    {
    }

    protected virtual void OnMatchEnding(List<ActivePlayer> players)
    {
    }

    protected virtual void OnPlayerEntering(ActivePlayer player)
    {
    }

    protected virtual void OnPlayerExiting(ActivePlayer player)
    {
    }
}