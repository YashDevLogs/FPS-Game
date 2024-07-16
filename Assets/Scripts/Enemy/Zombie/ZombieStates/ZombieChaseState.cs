using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    private Transform player;
    private NavMeshAgent agent;

    [SerializeField] private float ChaseSpeed = 6f;

    [SerializeField] private float StopChasingDistance = 21f;
    [SerializeField] private float AttackDistance = 2f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

       if(ServiceLocator.Instance.SoundManager.ZombieChannel.isPlaying == false)
        {
            ServiceLocator.Instance.SoundManager.ZombieChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.ZombieChase);
        }

        player = ServiceLocator.Instance.GlobalReference.PlayerTransfrom;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = ChaseSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float DistanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Check if agent should stop chasing
        if(DistanceFromPlayer > StopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        //check if agent should attack 

        if (DistanceFromPlayer < AttackDistance) 
        {
            animator.SetBool("isAttacking", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);

        ServiceLocator.Instance.SoundManager.ZombieChannel.Stop();
    }
}
