using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private CoinCollector _coinCollector;

    private void Start()
    {
        _coinCollector = FindObjectOfType<CoinCollector>();

        if (_coinCollector != null)
        {
            _coinCollector.CoinsChanged += UpdateCoinsDisplay;
            UpdateCoinsDisplay(_coinCollector.CurrentCoins, _coinCollector.RequiredCoins);
        }
    }

    private void OnDestroy()
    {
        if (_coinCollector != null)
        {
            _coinCollector.CoinsChanged -= UpdateCoinsDisplay;
        }
    }

    private void UpdateCoinsDisplay(int currentCoins, int requiredCoins)
    {
        _coinsText.text = $"{currentCoins}/{requiredCoins}";
    }
}