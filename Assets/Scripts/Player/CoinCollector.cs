using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private int _requiredCoins = 3;
    [SerializeField] private GameObject _door;

    private int _currentCoins;

    public int CurrentCoins => _currentCoins;
    public int RequiredCoins => _requiredCoins;

    public delegate void OnCoinsChanged(int currentCoins, int requiredCoins);
    public event OnCoinsChanged CoinsChanged;

    private void Start()
    {
        if (_door != null)
        {
            _door.SetActive(false);
        }

        CoinsChanged?.Invoke(_currentCoins, _requiredCoins);
    }

    public void AddCoin(int amount)
    {
        _currentCoins += amount;
        CoinsChanged?.Invoke(_currentCoins, _requiredCoins);

        if (_currentCoins >= _requiredCoins)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (_door != null)
        {
            _door.SetActive(true);
        }
    }
}