using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
     private Transform player;
    private NavMeshAgent agent;

    private float StopAttackingDistance = 2f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = ServiceLocator.Instance.GlobalReference.PlayerTransfrom;
        agent = animator.GetComponent<NavMeshAgent>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (ServiceLocator.Instance.SoundManager.ZombieChannel.isPlaying == false)
        {
            ServiceLocator.Instance.SoundManager.ZombieChannel.clip = ServiceLocator.Instance.SoundManager.ZombieWalking;
            ServiceLocator.Instance.SoundManager.ZombieChannel.PlayDelayed(1f);


        }

        LookAtPlayer();

        // check if it should stop attacking
        float DistanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Check if agent should stop chasing
        if (DistanceFromPlayer > StopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0,yRotation,0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ServiceLocator.Instance.SoundManager.ZombieChannel.Stop();
    }


}
