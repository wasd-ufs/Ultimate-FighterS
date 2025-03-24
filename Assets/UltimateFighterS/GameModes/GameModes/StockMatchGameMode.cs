
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class StockMatchGameMode : GameMode
{
    public UnityEvent<PlayerStock> onPlayerEntering = new();
    public UnityEvent<ActivePlayer> onPlayerExiting = new();
    
    private int initialStockCount = 3;
    private Dictionary<int, PlayerStock> stocks = new();

    protected override void OnMatchStarting()
    {
        stocks.Clear();
    }

    protected override void OnMatchEnding(List<ActivePlayer> players)
    {
        MatchResultManager.matchResults.Clear();

        foreach (var stock in stocks.Values)
        {
            var rank = stock.Rank;
            var port = stock.Player.Port;
            
            MatchResultManager.matchResults[rank] = port;
        }
    }

    protected override void OnPlayerEntering(ActivePlayer player)
    {
        stocks[player.Port] = new PlayerStock(player, initialStockCount);
        
        stocks[player.Port].onStockUpdated.AddListener(
            count => OnPlayerStockUpdated(stocks[player.Port], count)
        );
        
        stocks[player.Port].onStockZeroed.AddListener(
            () => OnPlayerStockZeroed(stocks[player.Port])
        );
        
        onPlayerEntering.Invoke(stocks[player.Port]);
    }

    protected override void OnPlayerExiting(ActivePlayer player)
    {
        stocks[player.Port].ClearEvents();
        
        onPlayerExiting.Invoke(player);
    }

    private void OnPlayerStockUpdated(PlayerStock stock, int count)
    {
        if (count <= 0)
            return;
        
        stock.Player.Spawn();
    }

    private void OnPlayerStockZeroed(PlayerStock stock)
    {
        stock.Rank = GetAmountOfPlayersWithStock();
        
        if (GetAmountOfPlayersWithStock() <= 1)
            MatchManager.EndMatch();
    }

    private int GetAmountOfPlayersWithStock() =>
        stocks.Values.ToList().ConvertAll(stock => stock.Stock > 0 ? 1 : 0).Sum();
}

public class PlayerStock
{
    public readonly UnityEvent<int> onStockUpdated = new();
    public readonly UnityEvent onStockZeroed = new();
    
    public ActivePlayer Player { get; private set; }
    public int Stock { get; private set; }
    public int Rank;

    public PlayerStock(ActivePlayer player, int stock)
    {
        Player = player;
        Stock = stock;
        Rank = 0;
        
        player.OnPlayerKilled.AddListener(_ => Decrease());
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
