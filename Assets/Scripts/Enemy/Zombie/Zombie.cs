using UnityEngine;

public class Zombie : MonoBehaviour
{
    // script attached on zombie prefab holding reference of zombie hand damage collider.
    [SerializeField] private ZombieHandDamage ZombieHand;
    public int ZombieDamage; // defines the damage whiich has to be dealt each time zombie hand collides with the player.

    void Start()
    {
        ZombieHand.damage = ZombieDamage; // sets the above defined damage to the zombie hand collider.
    }
}
