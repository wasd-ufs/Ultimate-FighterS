using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ActivePlayer
{
    public readonly UnityEvent<GameObject> OnPlayerKilled = new();
    public readonly UnityEvent<GameObject> OnPlayerSpawned = new();

    public ActivePlayer(int port, InputType input, Character character, Transform spawnPoint)
    {
        Port = port;
        Input = input;
        Character = character;
        SpawnPoint = spawnPoint;
        InGameObject = null;
    }

    public int Port { get; }
    public InputType Input { get; }
    public GameObject InGameObject { get; private set; }
    public Character Character { get; }
    public Transform SpawnPoint { get; }

    ~ActivePlayer()
    {
        OnPlayerSpawned.RemoveAllListeners();
        OnPlayerKilled.RemoveAllListeners();
    }

    public void Spawn()
    {
        if (InGameObject is not null)
            Object.Destroy(InGameObject);

        InGameObject =
            Object.Instantiate(Character.prefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);

        IdComponent idComponent = InGameObject.GetComponent<IdComponent>();
        if (idComponent is null)
        {
            Debug.LogError("No IdComponent attached to Player Prefab");

            Object.Destroy(InGameObject);
            InGameObject = null;
            return;
        }

        idComponent.id = Port;

        ProxyInputSystem proxy = InGameObject.GetComponent<ProxyInputSystem>();
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

        GameObject obj = InGameObject;
        OnPlayerSpawned.Invoke(obj);
    }

    public void Kill()
    {
        if (InGameObject is null)
            return;

        GameObject obj = InGameObject;

        Object.Destroy(InGameObject);
        InGameObject = null;

        OnPlayerKilled.Invoke(obj);
    }

    public bool IsInGameObjectAlive()
    {
        return InGameObject is not null;
    }
}