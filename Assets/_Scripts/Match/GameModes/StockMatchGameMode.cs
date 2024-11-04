using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class StockMatchGameMode : GameMode
{
    [SerializeField] private int initialStockCount = 3;
    private Dictionary<int, int> stockCounts = new();

    protected override void OnMatchStarting()
    {
        stockCounts.Clear();
    }

    protected override void OnPlayerEntering(ActivePlayer player)
    {
        stockCounts[player.Port] = initialStockCount;
    }

    protected override void OnPlayerExiting(ActivePlayer player)
    {
        stockCounts.Remove(player.Port);
    }

    protected override void OnPlayerKilled(ActivePlayer player)
    {
        stockCounts[player.Port]--;

        if (stockCounts[player.Port] <= 0)
        {
            MatchManager.RemovePlayer(player.Port);
            
            if (stockCounts.Count == 0)
                MatchManager.EndMatch();
            
            return;
        }
        
        MatchManager.RespawnPlayer(player.Port);
    }
}
