using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
    [SerializeField] private List<Earth> _earths = new List<Earth>();

    private int _countEarthBuy = 0;

    public void BuyEarth()
    {
        _countEarthBuy++;
    }

    public void LoadBoughtEarth(int countBoughtEarth)
    {
        for (int i = 0; i < countBoughtEarth; i++)
        {
            _earths[i].Buy();
        }
    }
}
