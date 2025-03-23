using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    public GameObject player;
    private TMP_Text _textElement;

    private void Awake()
    {
        _textElement = GetComponent<TMP_Text>();
        if (_textElement is null)
            return;

        DamageComponent damageComponent = player.GetComponent<DamageComponent>();

        damageComponent?.onDamageUpdate.AddListener(
            damage => _textElement.SetText($"{damage}")
        );
    }
}