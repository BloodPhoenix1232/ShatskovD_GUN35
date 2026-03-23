using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(health.CurrentHealth);
        }
    }
}