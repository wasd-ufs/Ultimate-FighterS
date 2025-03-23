using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class StockMatchGameMode : GameMode
{
    public UnityEvent<PlayerStock> onPlayerEntering = new();
    public UnityEvent<ActivePlayer> onPlayerExiting = new();

    private readonly int _initialStockCount = 3;
    private readonly Dictionary<int, PlayerStock> _stocks = new();

    protected override void OnMatchStarting()
    {
        _stocks.Clear();
    }

    protected override void OnMatchEnding(List<ActivePlayer> players)
    {
        MatchResult.Results.Clear();

        foreach (PlayerStock stock in _stocks.Values)
        {
            int rank = stock.Rank;
            int port = stock.Player.Port;

            MatchResult.Results[rank] = port;
        }
    }

    protected override void OnPlayerEntering(ActivePlayer player)
    {
        _stocks[player.Port] = new PlayerStock(player, _initialStockCount);

        _stocks[player.Port].OnStockUpdated.AddListener(
            count => OnPlayerStockUpdated(_stocks[player.Port], count)
        );

        _stocks[player.Port].OnStockZeroed.AddListener(
            () => OnPlayerStockZeroed(_stocks[player.Port])
        );

        onPlayerEntering.Invoke(_stocks[player.Port]);
    }

    protected override void OnPlayerExiting(ActivePlayer player)
    {
        _stocks[player.Port].ClearEvents();

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

    private int GetAmountOfPlayersWithStock()
    {
        return _stocks.Values.ToList().ConvertAll(stock => stock.Stock > 0 ? 1 : 0).Sum();
    }
}

public class PlayerStock
{
    public readonly UnityEvent<int> OnStockUpdated = new();
    public readonly UnityEvent OnStockZeroed = new();
    public int Rank;

    public PlayerStock(ActivePlayer player, int stock)
    {
        Player = player;
        Stock = stock;
        Rank = 0;

        player.OnPlayerKilled.AddListener(_ => Decrease());
    }

    public ActivePlayer Player { get; }
    public int Stock { get; private set; }

    public void Decrease()
    {
        Stock--;
        OnStockUpdated.Invoke(Stock);

        if (Stock <= 0)
            OnStockZeroed.Invoke();
    }

    public void ClearEvents()
    {
        OnStockUpdated.RemoveAllListeners();
        OnStockZeroed.RemoveAllListeners();
    }
}