using TMPro;
using UnityEngine;

public class PlayerStockUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private TMP_Text damageText;

    private PlayerStock _stock;

    public void Setup(PlayerStock stock)
    {
        _stock = stock;

        playerNameText.text = $"P{_stock.Player.Port + 1}";
        stockText.text = _stock.Stock.ToString();
        
        _stock.Player.OnPlayerSpawned.AddListener(OnPlayerSpawned);
        _stock.Player.OnPlayerKilled.AddListener(OnPlayerKilled);
        _stock.onStockUpdated.AddListener(OnStockUpdated);
        
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0.5f, 0f);
        rectTransform.anchorMin = new Vector2(0.5f, 0f);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        Debug.Log("spawn");
        damageText.text = "0%";
        
        var damageComponent = player.GetComponent<DamageComponent>();
        if (damageComponent is null)
        {
            Debug.Log("no damage component");
            return;
        }
        
        damageComponent.onDamageUpdate.AddListener(OnDamageUpdate);
    }

    private void OnPlayerKilled(GameObject player)
    {
        damageText.text = "0%";
    }
    
    private void OnStockUpdated(int stock)
    {
        stockText.text = stock.ToString();
    }

    private void OnDamageUpdate(float damage)
    {
        damageText.text = $"{Mathf.Floor(damage)}%";
    }
}