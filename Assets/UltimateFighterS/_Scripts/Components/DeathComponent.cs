using System;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Componente responsavel por aplicar morte em um jogador.
///</summary>
public class DeathComponent : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnDeath;
    
    /// <summary>
    /// Responsavel por matar e acionar o evento de morte do player.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    public void Kill()
    {
        OnDeath.Invoke();
        ActivePlayer _player = MatchManager.GetPlayer(gameObject);
        _player.Kill();
    }
    
    /// <summary>
    /// Remove todos os nao-persistentes ouvintes do evento OnDeath.
    /// </summary>
    /// <author>JOÃO CARLOS</author>
    private void OnDestroy()
    {
        OnDeath.RemoveAllListeners();
    }
}