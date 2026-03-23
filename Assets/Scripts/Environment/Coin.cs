using UnityEngine;

public class Coin : MonoBehaviour
{
    private int _value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<CoinCollector>(out CoinCollector collector))
        {
            collector.AddCoin(_value);
            Destroy(gameObject);
        }
    }
}