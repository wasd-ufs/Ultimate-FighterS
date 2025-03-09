using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Controla o recebimento de dano do jogador.
///</summary>
public class DamageComponent : MonoBehaviour
{
    [SerializeField] [ReadOnly] public float currentDamage = 0f;
    [HideInInspector] public UnityEvent<float> onDamageUpdate = new();
    
    public float CurrentDamage => currentDamage;

    ///<summary>
    /// Aplica o dano no jogador.
    ///<param name="damage"> A quantidade de dano a ser aplicada.
    ///</summary>
    ///<author>Davi Fontes</author>
    public void TakeDamage(float damage)
    {
        currentDamage += damage;
        onDamageUpdate.Invoke(currentDamage);
    }

    public void OnDestroy()
    {
        onDamageUpdate.RemoveAllListeners();
    }
}