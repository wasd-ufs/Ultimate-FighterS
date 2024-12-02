using System;
using UnityEngine;
using UnityEngine.Events;
public class DeathComponent : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDeath;

    public void Kill()
    {
        onDeath.Invoke();
        
        var player = MatchManager.GetPlayer(gameObject);
        player.Kill();
    }

    private void OnDestroy()
    {
        onDeath.RemoveAllListeners();
    }
}