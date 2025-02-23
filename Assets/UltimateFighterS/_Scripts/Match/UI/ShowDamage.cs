using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    public GameObject player;
    private TMP_Text textElement;

    private void Awake()
    {
        textElement = GetComponent<TMP_Text>();
        if (textElement is null)
            return; 
        
        var damageComponent = player.GetComponent<DamageComponent>();

        damageComponent?.onDamageUpdate.AddListener(
            damage => textElement.SetText($"{damage}")
        );
    }
}
