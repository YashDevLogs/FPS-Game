using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private ZombieHandDamage ZombieHand;
    public int ZombieDamage;

    void Start()
    {
        ZombieHand.damage = ZombieDamage;
    }


}
