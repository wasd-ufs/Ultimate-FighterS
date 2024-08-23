using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class StockMatchManager : MatchManager
{
    [SerializeField] private int numberOfStocks;
    [SerializeField] private int numberOfPlayers; //placeholder 
    [SerializeField] private Dictionary<int, int> keyValuePairs;

    private void Awake()
    {
        GlobalEvents.onPlayerDied.AddListener(reduceStock);
        keyValuePairs = new Dictionary<int, int>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            keyValuePairs.Add(i, numberOfStocks);
        }
    }

    private void reduceStock(GameObject gameObject)
    {
        gameObject.TryGetComponent<IdComponent>(out IdComponent idComponent);
        int currentPlayerId = idComponent.id;

        if (keyValuePairs.TryGetValue(currentPlayerId, out int remainingStocks))
        {
            keyValuePairs[currentPlayerId] = remainingStocks--;

        }
    }
}
