using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MockMatchAssembler : MonoBehaviour
{
    [ReadOnly] private const string PlayerTag = "Player";
    
    public void Start()
    {
        MatchManager.OnMatchEnding.AddListener(_ => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        MatchManager.StartMatch();

        var prefabs = GetPlayers();

        int port = 0;
        foreach (var prefab in prefabs)
        {
            MatchManager.AddPlayer(port, InputType.Player, prefab, prefab.transform);
            port++;
        }
        
        MatchManager.ForEachPlayer(player => player.Spawn());
    }

    public List<GameObject> GetPlayers() => GameObject.FindGameObjectsWithTag(PlayerTag).ToList();
}