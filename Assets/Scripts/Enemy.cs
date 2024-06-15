using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float Health = 100f;
    private Animator ZombieAnim;

    private NavMeshAgent navAgent;

    void Start()
    {
        ZombieAnim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    internal void TakeDamage(float damageAmt)
    {
       Health -= damageAmt;

        if (Health <= 0)
        {
            int randomValue = Random.Range(0,2);

            if(randomValue == 0 )
            {
                ZombieAnim.SetTrigger("DIE1");
            }
            else
            {
                ZombieAnim.SetTrigger("DIE2");
            }       
        }
        else
        {
            ZombieAnim.SetTrigger("DAMAGE");
        }
    }

    void Update()
    {
        if(navAgent.velocity.magnitude > 0.1f)
        {
            ZombieAnim.SetBool("isWalking", true);      
        }
        else
        {
            ZombieAnim.SetBool("isWalking", false);
        }
    }
}
