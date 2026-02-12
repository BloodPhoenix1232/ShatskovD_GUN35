using UnityEngine;
using UnityEngine.AI;

public class Collect : StateMachineBehaviour
{
    public float collectDistance = 1.5f;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AIController ai = animator.GetComponent<AIController>();
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();

        GameObject target = ai.GetTargetItem();
        if (target == null) return;

        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

        if (Vector3.Distance(animator.transform.position, target.transform.position) < collectDistance)
        {
            GameObject.Destroy(target);
            ai.ClearTarget();
            animator.SetBool("IsCollecting", false);
            animator.SetTrigger("Collected");
        }
    }
}
