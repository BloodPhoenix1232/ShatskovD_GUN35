using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _lifeTime = 3f;

    private ObjectPool _pool;

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(_damage);
            ReturnToPool();
        }
        else if (IsGround(other))
        {
            ReturnToPool();
        }
    }

    private bool IsGround(Collider2D other)
    {
        return ((1 << other.gameObject.layer) & _groundLayer) != 0;
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetPool(ObjectPool pool)
    {
        _pool = pool;
    }
}