using UnityEngine;
using UnityEngine.Events;

public class DeathComponent : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDeath;

    private void OnDestroy()
    {
        onDeath.RemoveAllListeners();
    }

    public void Kill()
    {
        onDeath.Invoke();

        ActivePlayer player = MatchManager.GetPlayer(gameObject);
        player.Kill();
    }
}