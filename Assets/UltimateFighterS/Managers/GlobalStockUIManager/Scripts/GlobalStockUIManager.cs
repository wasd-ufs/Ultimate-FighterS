using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StockMatchGameMode))]
public class GlobalStockUIManager : MonoBehaviour
{
    [SerializeField] private float height;

    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject playerUIPrefab;
    private StockMatchGameMode _stockMatchGameMode;

    private readonly Dictionary<int, PlayerStockUI> _stockUIs = new();

    private void Awake()
    {
        _stockMatchGameMode = GetComponent<StockMatchGameMode>();
        
        _stockMatchGameMode.onPlayerEntering.AddListener(OnPlayerEntering);
        _stockMatchGameMode.onPlayerExiting.AddListener(OnPlayerExiting);
    }

    private void OnPlayerEntering(PlayerStock stock)
    {
        Debug.Log($"OnPlayerEntering: {stock.Player.Port}");
        var ui = Instantiate(playerUIPrefab, canvas, true);
        
        var stockUi = ui.GetComponent<PlayerStockUI>();
        if (stockUi is null)
        {
            Debug.LogError("Stock UI prefab does not have a PlayerStockUI component");
            return;
        }
        
        _stockUIs[stock.Player.Port] = stockUi;
        stockUi.Setup(stock);
        ReorganizeUI();
    }

    private void ReorganizeUI()
    {
        var start = 0;
        var end = Screen.width;
        var spacing = (end - start) / (_stockUIs.Count + 1);

        int i = 0;
        foreach (var ui in _stockUIs.Values)
        {
            ui.transform.position = new Vector2(start + (i + 1) * spacing, height);
            i++;
        }
    }

    private void OnPlayerExiting(ActivePlayer player)
    {
        if (_stockUIs.ContainsKey(player.Port))
            return;
        
        Destroy(_stockUIs[player.Port]);
        _stockUIs.Remove(player.Port);
        
        ReorganizeUI();
    }
}