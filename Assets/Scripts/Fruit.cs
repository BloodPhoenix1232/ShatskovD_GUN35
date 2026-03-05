using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitObject fruitObject;

    private Basket _basket;

    public Basket Basket => _basket;

    public int Price => fruitObject.Price;

    public FruitObject FruitObject => fruitObject;

    public void Init(Basket basket)
    {
        _basket = basket;
    }
}
