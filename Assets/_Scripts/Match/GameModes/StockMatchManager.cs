using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class StockMatchGameMode : GameMode
{
    [SerializeField] private int initialStockCount = 3;
    
    private Dictionary<int, int> stockCounts = new();

    protected override void OnPlayerEntering(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;

        stockCounts[idComponent.id] = initialStockCount;
    }

    protected override void OnPlayerExiting(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;
        
        stockCounts.Remove(idComponent.id);
    }

    protected override void OnPlayerDeath(GameObject player)
    {
        var idComponent = player.GetComponent<IdComponent>();
        if (idComponent is null)
            return;

        stockCounts[idComponent.id]--;

        if (stockCounts[idComponent.id] <= 0)
        {
            MatchManager.RemovePlayer(player);
            
            if (stockCounts.Count == 0)
                MatchManager.EndMatch();
            
            return;
        }
        
        MatchManager.RespawnPlayer(player);
    }
}
