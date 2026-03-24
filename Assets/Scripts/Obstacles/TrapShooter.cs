using UnityEngine;

public class TrapShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private ObjectPool _projectilePool;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootInterval = 2f;
    [SerializeField] private float _projectileSpeed = 5f;

    private float _timer;

    private void Start()
    {
        _timer = _shootInterval;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Shoot();
            _timer = _shootInterval;
        }
    }

    private void Shoot()
    {
        if (_projectilePool == null || _shootPoint == null) return;

        GameObject projectile = _projectilePool.GetObject(_shootPoint.position, Quaternion.identity);

        projectile.transform.rotation = Quaternion.Euler(0, 0, -90);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.up * _projectileSpeed;
        }

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetPool(_projectilePool);
        }
    }
}