using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _shootPoint;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _shootInterval = 1.5f;
    [SerializeField] private float _projectileSpeed = 5f;

    [SerializeField] private ObjectPool _projectilePool;

    private Transform _player;
    private float _shootTimer;

    private void Start()
    {
        _shootTimer = _shootInterval;
    }

    private void Update()
    {
        FindPlayer();

        if (_player != null)
        {
            float distance = Vector2.Distance(transform.position, _player.position);

            if (distance <= _detectionRadius)
            {
                _shootTimer -= Time.deltaTime;

                if (_shootTimer <= 0)
                {
                    Shoot();
                    _shootTimer = _shootInterval;
                }
            }
        }
    }

    private void FindPlayer()
    {
        if (_player == null)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
            if (hit != null)
            {
                _player = hit.transform;
            }
        }
    }

    private void Shoot()
    {
        if (_projectilePrefab == null || _shootPoint == null) return;
        if (_player == null) return;

        if (_projectilePool == null)
        {
            _projectilePool = FindObjectOfType<ObjectPool>();
        }

        GameObject projectile;

        if (_projectilePool != null)
        {
            projectile = _projectilePool.GetObject(_shootPoint.position, Quaternion.identity);
        }
        else
        {
            projectile = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
        }

        Vector2 direction = (_player.position - _shootPoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * _projectileSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        EnemyProjectile projScript = projectile.GetComponent<EnemyProjectile>();
        if (projScript != null)
        {
            projScript.SetDamage(1);

            if (_projectilePool != null)
                projScript.SetPool(_projectilePool);
        }
    }

    public void TakeDamage(int damage)
    {
        Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}