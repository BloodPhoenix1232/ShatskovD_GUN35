using TMPro;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsCountText;

    private void OnEnable()
    {
        Coin.OnChangeNumberCoins += ShowCountCoins;
    }

    private void OnDisable()
    {
        Coin.OnChangeNumberCoins -= ShowCountCoins;
    }

    private void ShowCountCoins(int coinCount)
    {
        _coinsCountText.text = coinCount.ToString();
    }
}
