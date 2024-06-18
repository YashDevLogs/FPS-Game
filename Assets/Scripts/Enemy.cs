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
    public bool isDead = false;

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
            
            int randomValue = Random.Range(0, 2);

            if (randomValue == 0)
            {
                ZombieAnim.SetTrigger("DIE1");
            }
            else
            {
                ZombieAnim.SetTrigger("DIE2");
            }
            isDead = true;
            SoundManager.Instance.ZombieChannel2.PlayOneShot(SoundManager.Instance.ZombieDeath);
        }
        else 
        {
            ZombieAnim.SetTrigger("DAMAGE");
            SoundManager.Instance.ZombieChannel2.PlayOneShot(SoundManager.Instance.ZombieHurt);

        }
    }

    void Update()
    {

    }
}
