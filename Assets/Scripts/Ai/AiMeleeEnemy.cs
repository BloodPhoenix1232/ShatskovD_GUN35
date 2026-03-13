using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AiSensor))]
public class MeleeEnemy : MonoBehaviour
{
    [Header("Ссылки")]
    public AiSensor sensor;
    private NavMeshAgent agent;

    [Header("Параметры атаки")]
    public float attackRange = 1.5f;  // Дистанция для атаки
    public float attackCooldown = 1f; // Задержка между ударами
    public int damage = 10;           // Урон игроку

    private GameObject target;
    private float lastAttackTime;
    public GameObject knifePrefab;

    private GameObject knife;
    private MeshSockets sockets;
    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sockets = GetComponent<MeshSockets>();
        animator = GetComponent<Animator>();

        knife = Instantiate(knifePrefab);

        if (!sensor) sensor = GetComponent<AiSensor>();
    }

    void Update()
    {
        if (isDead) return;

        if (sensor.Objects.Count > 0)
        {
            target = sensor.Objects[0];
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (sockets != null && knife != null)
            {
                sockets.Attach(knife.transform, MeshSockets.SocketId.RightHand);
                knife.transform.localEulerAngles = new Vector3(0f, 125f, -65f);
            }

            if (distance > attackRange)
            {
                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(target.transform.position);
                }
            }
            else
            {
                Attack();
            }
        }
        else
        {
            target = null;
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
            }
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("attack_knife");
        }

        if (target != null)
        {
            Health playerHealth = target.GetComponent<Health>();
            if (playerHealth != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                playerHealth.TakeDamage(damage, direction);
            }
        }
    }

    public void Die()
    {
        isDead = true;

        if (agent != null) agent.enabled = false;

        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        if (knife != null)
        {
            knife.SetActive(false);
        }

        Destroy(gameObject, 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}