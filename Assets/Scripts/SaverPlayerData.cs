using UnityEngine;

public class SaverPlayerData : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private SpawnPlant _spawnplant;

    public void SaveCoin()
    {
        PlayerPrefs.SetInt("Coin", _wallet.Coins);
    }
}
