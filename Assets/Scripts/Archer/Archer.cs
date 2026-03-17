using UnityEngine;
using Netologia.TowerDefence;
using Netologia.Systems;

public class Archer : MonoBehaviour
{
    public float Speed = 3f;
    public float Range = 4f;
    public float AttackCooldown = 1.5f;
    public float Damage = 10f;

    public Projectile ProjectilePrefab;
    public Transform ShootPoint;
    public ElementalType ElementalType = ElementalType.Physic;

    public float SeparationDistance = 0.5f;
    public LayerMask ObstacleLayer;
    public float ObstacleCheckRadius = 0.2f;

    public Unit Target { get; private set; }

    private float _attackTimer;
    private UnitSystem _units;
    private ProjectileSystem _projectiles;

    public void Initialize(UnitSystem units, ProjectileSystem projectiles)
    {
        _units = units;
        _projectiles = projectiles;
        _attackTimer = 0f;
    }

    private void Update()
    {
        if (_units == null || _projectiles == null) return;

        float deltaTime = Time.deltaTime;

        if (Target == null || Target.CurrentHealth <= 0)
        {
            Target = _units.FindTarget(transform.position, float.MaxValue);
            if (Target == null) return;
        }

        Vector2 toTarget = (Target.transform.position - transform.position);
        float distance = toTarget.magnitude;

        if (distance > Range)
        {
            MoveTowardsTarget(toTarget.normalized, deltaTime);
        }

        if (distance <= Range)
        {
            _attackTimer -= deltaTime;
            if (_attackTimer <= 0f)
            {
                _attackTimer = AttackCooldown;
                var proj = _projectiles[ProjectilePrefab].Get;
                proj.PrepareData(ShootPoint.position, Target, Damage, ElementalType);
            }
        }
    }

    private void MoveTowardsTarget(Vector2 moveDir, float deltaTime)
    {
        foreach (var archer in FindObjectsOfType<Archer>())
        {
            if (archer == this) continue;

            float dist = Vector2.Distance(archer.transform.position, transform.position);
            if (dist < SeparationDistance)
            {
                Vector2 away = ((Vector2)transform.position - (Vector2)archer.transform.position).normalized;
                moveDir += away;
            }
        }

        moveDir.Normalize();

        Vector2 newPos = (Vector2)transform.position + moveDir * Speed * deltaTime;
        Collider2D hit = Physics2D.OverlapCircle(newPos, ObstacleCheckRadius, ObstacleLayer);

        if (hit == null)
        {
            transform.position += (Vector3)(moveDir * Speed * deltaTime);
        }
    }
}