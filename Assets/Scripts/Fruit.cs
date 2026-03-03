using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Basket _basket;

    public Basket Basket => _basket;

    public void Init(Basket basket)
    {
        _basket = basket;
    }
}
