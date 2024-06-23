using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHandDamage ZombieHand;
    public int ZombieDamage;

    void Start()
    {
        ZombieHand.damage = ZombieDamage;
    }


}
