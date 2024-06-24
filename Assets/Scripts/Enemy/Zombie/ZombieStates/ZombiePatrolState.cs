using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : StateMachineBehaviour
{
    float timer;
    public float PatrolTimer = 0f;

    Transform player;
    NavMeshAgent agent;

    public float DetectionArea = 18f;
    public float PatrolSpeed = 2f;

    List<Transform> waypoints = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(ServiceLocator.Instance.SoundManager.ZombieChannel.isPlaying == false)
        {
            ServiceLocator.Instance.SoundManager.ZombieChannel.clip = ServiceLocator.Instance.SoundManager.ZombieWalking;
            ServiceLocator.Instance.SoundManager.ZombieChannel.PlayDelayed(1f);

        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = PatrolSpeed;
        timer = 0;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach(Transform t in waypointCluster.transform)
        {
            waypoints.Add(t);
        }

        Vector3 nextPosition = waypoints[Random.Range(0, waypoints.Count)].position;
        agent.SetDestination(nextPosition);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance) 
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);     
        }

        // transition to idle state

        timer = +Time.deltaTime;
        if(timer > PatrolTimer) 
        {
            animator.SetBool("isPatroling", false);
        }

        // transition to chasing state

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < DetectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        ServiceLocator.Instance.SoundManager.ZombieChannel.Stop();

    }
}
