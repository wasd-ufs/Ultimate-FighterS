using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchConfiguration : MonoBehaviour
{
    public static MatchConfiguration Instance { get; private set; }

    // Prefabs
    public GameObject ScenePrefab;
    public GameObject ManagerPrefab;
    public List<GameObject> PlayersPrefabs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}