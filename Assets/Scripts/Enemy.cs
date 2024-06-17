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
    private bool isDead = false;

    private NavMeshAgent navAgent;

    void Start()
    {
        ZombieAnim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damageAmt)
    {
        
       Health -= damageAmt;

        if (Health <= 0)
        {
            isDead = true;
            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                ZombieAnim.SetTrigger("DIE1");
            }
            else
            {
                ZombieAnim.SetTrigger("DIE2");
            }
        }
        else if(!isDead)
        {
            ZombieAnim.SetTrigger("DAMAGE");
            
        }
    }

    void Update()
    {

    }
}
