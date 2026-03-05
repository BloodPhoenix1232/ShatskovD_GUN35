using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderPlayerData : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private int _coinCount;

    private void Start()
    {
        LoadCoins();
    }

    public void LoadCoins()
    {
        if (PlayerPrefs.HasKey("Coin"));
        {
            _coinCount = PlayerPrefs.GetInt("Coin");
            _wallet.LoadCoin(_coinCount);
        }
    }
}
