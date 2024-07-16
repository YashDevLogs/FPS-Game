using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    private float timer;
    [SerializeField] private float IdleTime = 0f;
    private Transform player;
    [SerializeField] private float DetectionAreaRadius = 18f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = ServiceLocator.Instance.GlobalReference.PlayerTransfrom;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // transition to patrol state

        timer += Time.deltaTime;
        if (timer > IdleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        // transition to chasing state

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < DetectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

}
