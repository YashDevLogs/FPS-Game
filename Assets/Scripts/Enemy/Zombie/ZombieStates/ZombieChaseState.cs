using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    Transform player;
    NavMeshAgent agent;

    public float ChaseSpeed = 6f;

    public float StopChasingDistance = 21f;
    public float AttackDistance = 2f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(ServiceLocator.Instance.SoundManager.ZombieChannel.isPlaying == false)
        {
            ServiceLocator.Instance.SoundManager.ZombieChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.ZombieChase);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
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
