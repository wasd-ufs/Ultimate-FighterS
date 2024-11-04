using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    public GameObject player;
    private TMP_Text textElement;
    private DamageComponent damageComponent;

    private void Awake()
    {
        player.TryGetComponent<DamageComponent>(out DamageComponent damageComponent);
    }
    private void Update()
    {
        textElement.SetText(damageComponent.currentDamage.ToString());
    }
}
