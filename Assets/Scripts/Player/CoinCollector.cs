using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private int _requiredCoins = 3;
    [SerializeField] private GameObject _door;

    private int _currentCoins;
    private static int _totalCoins;

    public int CurrentCoins => _currentCoins;
    public int RequiredCoins => _requiredCoins;
    public static int TotalCoins => _totalCoins;

    public delegate void OnCoinsChanged(int currentCoins, int requiredCoins);
    public event OnCoinsChanged CoinsChanged;

    private void Start()
    {
        if (_door != null)
        {
            _door.SetActive(false);
        }

        UpdateTotalCoins();
        CoinsChanged?.Invoke(_currentCoins, _requiredCoins);
    }

    public void AddCoin(int amount)
    {
        _currentCoins += amount;
        _totalCoins += amount;
        CoinsChanged?.Invoke(_currentCoins, _requiredCoins);

        if (_currentCoins >= _requiredCoins)
        {
            OpenDoor();
        }

        SaveTotalCoins();
    }

    private void OpenDoor()
    {
        if (_door != null)
        {
            _door.SetActive(true);
        }
    }

    private void UpdateTotalCoins()
    {
        SaveData data = SaveSystem.Instance?.LoadGame();
        if (data != null)
        {
            _totalCoins = data.totalCoins;
        }
    }

    private void SaveTotalCoins()
    {
        SaveData data = SaveSystem.Instance?.LoadGame();
        int unlockedLevel = data?.unlockedLevel ?? 1;
        SaveSystem.Instance?.SaveGame(_totalCoins, unlockedLevel);
    }
}