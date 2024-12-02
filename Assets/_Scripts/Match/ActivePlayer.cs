using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ActivePlayer
{
    public readonly UnityEvent<GameObject> OnPlayerSpawned = new();
    public readonly UnityEvent<GameObject> OnPlayerKilled = new();
    
    public int Port { get; private set; }
    public InputType Input { get; private set; }
    public GameObject InGameObject { get; private set; }
    public GameObject Prefab { get; private set; }
    public Transform SpawnPoint { get; private set; }

    public ActivePlayer(int port, InputType input, GameObject prefab, Transform spawnPoint)
    {
        Port = port;
        Input = input;
        Prefab = prefab;
        SpawnPoint = spawnPoint;
        InGameObject = null;
    }

    ~ActivePlayer()
    {
        OnPlayerSpawned.RemoveAllListeners();
        OnPlayerKilled.RemoveAllListeners();
    }

    public void Spawn()
    {
        if (InGameObject is not null)
            Object.Destroy(InGameObject);
        
        InGameObject = Object.Instantiate(Prefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation, SpawnPoint.parent);
        var idComponent = InGameObject.GetComponent<IdComponent>();
        if (idComponent is null)
        {
            Debug.LogError("No IdComponent attached to Player Prefab");
            
            Object.Destroy(InGameObject);
            InGameObject = null;
            return;
        }
        
        idComponent.id = Port;

        var proxy = InGameObject.GetComponent<ProxyInputSystem>();
        if (proxy is null)
        {
            Debug.LogError("No ProxyInputSystem attached to Player Prefab");
            
            Object.Destroy(InGameObject);
            InGameObject = null;
            return;
        }

        proxy.input = Input switch
        {
            InputType.Player => InGameObject.AddComponent<PlayerInputSystem>(),
            InputType.NoInput => InGameObject.AddComponent<NoInputSystem>(),
            _ => null
        };

        var obj = InGameObject;
        OnPlayerSpawned.Invoke(obj);
    }

    public void Kill()
    {
        if (InGameObject is null)
            return;

        var obj = InGameObject;
        
        Object.Destroy(InGameObject);
        InGameObject = null;
        
        OnPlayerKilled.Invoke(obj);
    }
    
    public bool IsInGameObjectAlive() => InGameObject is not null;
}