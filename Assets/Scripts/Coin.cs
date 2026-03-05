using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private SaverPlayerData _playerData;
    private int _coins;

    public static Action<int> OnChangeNumberCoins;

    public int Coins => _coins;

    public void IncreaseValue(int valueIncrease)
    {
        _coins += valueIncrease;
        OnChangeNumberCoins?.Invoke(_coins);
        _playerData.SaveCoin();
    }

    public bool DecreaseValue(int cost)
    {
        if (_coins >= cost)
        {
            _coins -= cost;
            OnChangeNumberCoins?.Invoke(_coins);
            _playerData.SaveCoin();
            return true;
        }
        return false;
    }
}
