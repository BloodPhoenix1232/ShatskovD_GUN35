using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _damageCooldown = 1f;

    [Header("Drop")]
    [SerializeField] protected int _diamondsDrop = 1;

    protected float _lastDamageTime;

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            if (Time.time - _lastDamageTime >= _damageCooldown)
            {
                health.TakeDamage(_damage);
                _lastDamageTime = Time.time;
            }
        }
    }

    protected virtual void Die()
    {
        DropDiamonds();
        Destroy(gameObject);
    }

    protected void DropDiamonds()
    {
        if (DiamondManager.Instance != null)
        {
            DiamondManager.Instance.AddDiamonds(_diamondsDrop);
        }
    }
}