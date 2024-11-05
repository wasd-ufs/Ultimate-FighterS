using UnityEngine;
using UnityEngine.Events;

public class GlobalEvents
{
    // Player Related
    public static readonly UnityEvent<GameObject, Vector2, float> OnPlayerHit = new();
    public static readonly UnityEvent<GameObject> OnPlayerDied = new();
    
    
    // Match Related
    public static readonly UnityEvent OnMatchStarted = new();
}
