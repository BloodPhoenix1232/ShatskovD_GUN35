using System;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    [SerializeField] private EarthController _earthController;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private int _price;

    [SerializeField] private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private bool _isBuy;

    public static Action<List<SpawnPoint>> OnBoughtEarthList;
    private bool _isBoughtEarth;

    public int Price => _price;
        
    public bool IsBoughtEarth => _isBoughtEarth;

    public void Buy()
    {
        _isBuy = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;

        foreach (SpawnPoint spawnPoint in _spawnPoints)
        {
            spawnPoint.gameObject.SetActive(true);
        }

        OnBoughtEarthList?.Invoke(_spawnPoints);
        _isBoughtEarth = true;
        _earthController.BuyEarth();
    }

    private void OnMouseDown()
    {
        if(_wallet.GiveAway(_price))
        {
            Buy();
        }
    }
}
