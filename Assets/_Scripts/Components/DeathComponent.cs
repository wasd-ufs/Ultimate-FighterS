using System;
using UnityEngine;
using UnityEngine.Events;
public class DeathComponent : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDeath;

    public void Kill()
    {
        onDeath.Invoke();
        MatchManager.KillPlayer(gameObject);
    }

    private void OnDestroy()
    {
        onDeath.RemoveAllListeners();
    }
}