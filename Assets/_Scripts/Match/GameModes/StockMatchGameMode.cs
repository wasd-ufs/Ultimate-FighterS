using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;

public class StockMatchGameMode : GameMode
{
    public UnityEvent<PlayerStock> onPlayerEntering = new();
    public UnityEvent<ActivePlayer> onPlayerExiting = new();
    public UnityEvent<PlayerStock> onStockUpdated = new();
    
    private int initialStockCount = 3;
    private Dictionary<int, PlayerStock> stocks = new();

    protected override void OnMatchStarting()
    {
        stocks.Clear();
    }

    protected override void OnPlayerEntering(ActivePlayer player)
    {
        stocks[player.Port] = new PlayerStock(player, initialStockCount);
        
        stocks[player.Port].onStockUpdated.AddListener(
            _ => onStockUpdated.Invoke(stocks[player.Port])
        );
        
        stocks[player.Port].onStockZeroed.AddListener(
            () => MatchManager.RemovePlayer(player.Port)
        );
        
        onPlayerEntering.Invoke(stocks[player.Port]);
    }

    protected override void OnPlayerExiting(ActivePlayer player)
    {
        stocks[player.Port].ClearEvents();
        
        onPlayerExiting.Invoke(player);
        stocks.Remove(player.Port);
    }

    protected override void OnPlayerKilled(ActivePlayer player)
    {
        if (!stocks.ContainsKey(player.Port))
            return;
        
        stocks[player.Port].Decrease();
        
        if (stocks.Count <= 1)
            MatchManager.EndMatch();
        
        MatchManager.SpawnPlayer(player.Port);
    }
}

public class PlayerStock
{
    public readonly UnityEvent<int> onStockUpdated = new();
    public readonly UnityEvent onStockZeroed = new();
    
    public ActivePlayer Player { get; private set; }
    public int Stock { get; private set; }

    public PlayerStock(ActivePlayer player, int stock)
    {
        Player = player;
        Stock = stock;
    }

    public void Decrease()
    {
        Stock--;
        onStockUpdated.Invoke(Stock);
        
        if (Stock <= 0)
            onStockZeroed.Invoke();
    }

    public void ClearEvents()
    {
        onStockUpdated.RemoveAllListeners();
        onStockZeroed.RemoveAllListeners();
    }
}
