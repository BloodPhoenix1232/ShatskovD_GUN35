using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(_damage);
        }
    }
}