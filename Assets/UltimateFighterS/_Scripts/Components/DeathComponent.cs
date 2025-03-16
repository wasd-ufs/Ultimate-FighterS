using System;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Componente responsavel por aplicar morte em um jogador.
///</summary>
public class DeathComponent : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDeath;
    /// <summary>
    /// Responsavel por matar e acionar o evento de morte do player.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    public void Kill()
    {
        onDeath.Invoke();
        var player = MatchManager.GetPlayer(gameObject);
        player.Kill();
    }
    /// <summary>
    /// Remove todos os nao-persistentes ouvintes do evento onDeath.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    private void OnDestroy()
    {
        onDeath.RemoveAllListeners();
    }
}