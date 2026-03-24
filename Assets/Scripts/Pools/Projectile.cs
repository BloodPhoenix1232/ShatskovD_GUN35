using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifetime = 3f;

    private ObjectPool _pool;
    private float _lifeTimer;

    private void OnEnable()
    {
        _lifeTimer = _lifetime;
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    private void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(_damage);

            ReturnToPool();
        }
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

    public void SetPool(ObjectPool pool)
    {
        _pool = pool;
    }
}