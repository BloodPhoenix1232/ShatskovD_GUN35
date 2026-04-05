using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _groundLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<MeleeEnemy>(out MeleeEnemy enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent<ShootingEnemy>(out ShootingEnemy shootingEnemy))
        {
            shootingEnemy.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        if (IsGround(other))
        {
            Destroy(gameObject);
        }
    }

    private bool IsGround(Collider2D other)
    {
        return ((1 << other.gameObject.layer) & _groundLayer) != 0;
    }
}