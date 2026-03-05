using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private Earth _earth;

    private string _score = "Score";

    private int _currentMultiplie = 1;
    private int _counterReceivingMoney;
    private int _rightAmountAd = 10;

    public static Action OnEnableTutorialEarth;

    public string Score => _score;
    public int Coins => _coin.Coins;

    private void OnEnable()
    {
        FruitMovement.OnCut += IncreaseMoneyCuttingFruit;
    }

    private void OnDisable()
    {
        FruitMovement.OnCut -= IncreaseMoneyCuttingFruit;
    }

    public void IncreaseMoneyCuttingFruit(Fruit fruit)
    {
        _counterReceivingMoney++;
        int valueIncrease = fruit.Price * _currentMultiplie;

        _coin.IncreaseValue(valueIncrease);

        LearnAboutShowTutorialEarth();
    }

    private void LearnAboutShowTutorialEarth()
    {
        if (_earth.Price == _coin.Coins)
        {
            OnEnableTutorialEarth?.Invoke();
        }
    }

    public bool GiveAway(int price)
    {
        if (_coin.DecreaseValue(price))
        {
            return true;
        }
        return false;
    }

    public bool TryPriceIncrease(int value, int cost)
    {
        if (_coin.DecreaseValue(cost))
        {
            PriceIncrease(value);
            return true;
        }
        return false;
    }

    public void PriceIncrease(int value)
    {
        _currentMultiplie = value;
    }

    public void GiveAdReward(int rewardAmount)
    {
        _coin.IncreaseValue(_coin.Coins);
    }

    public void LoadCoin(int coin)
    {
        _coin.IncreaseValue(coin);
    }
}
