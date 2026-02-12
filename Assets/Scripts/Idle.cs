using UnityEngine;
using UnityEngine.AI;

public class Idle : StateMachineBehaviour
{
    public float idleTime = 5f;
    private float timer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = true;

        AIController ai = animator.GetComponent<AIController>();
        ai.canSearch = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        AIController ai = animator.GetComponent<AIController>();

        if (timer >= idleTime && !animator.GetBool("IsSearching"))
        {
            animator.SetBool("IsSearching", true);
            ai.canSearch = true;
        }
    }
}
