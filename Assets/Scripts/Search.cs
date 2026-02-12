using UnityEngine;
using UnityEngine.AI;

public class Search : StateMachineBehaviour
{
    public float wanderRadius = 10f;
    private Vector3 targetPos;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;

        targetPos = GetRandomPoint(animator.transform.position, wanderRadius);
        agent.SetDestination(targetPos);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        AIController ai = animator.GetComponent<AIController>();

        if (!ai.canSearch) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            targetPos = GetRandomPoint(animator.transform.position, wanderRadius);
            agent.SetDestination(targetPos);
        }

        Collider[] hits = Physics.OverlapSphere(animator.transform.position, ai.searchRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Item"))
            {
                ai.SetTarget(hit.gameObject);
                animator.SetBool("IsCollecting", true);
                animator.SetBool("IsSearching", false);
                break;
            }
        }
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir += center;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, radius, NavMesh.AllAreas))
            return hit.position;
        return center;
    }
}
