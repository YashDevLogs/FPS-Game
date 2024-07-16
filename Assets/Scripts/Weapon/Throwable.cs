using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 10f;
    [SerializeField] private float explosionForce = 1200f;

    private float countdown;

    private bool hasExploded = false;
    public bool HasBeenThrown = false; // need referenece of this bool in Weapon Manager.


    public enum ThrowableType
    {
        Grenade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (HasBeenThrown)
        {
            countdown-= Time.deltaTime;
            if(countdown <= 0f && !hasExploded )
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {

        GameObject explosionEffect = ServiceLocator.Instance.GlobalReference.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        ServiceLocator.Instance.SoundManager.ThrowableChannel.PlayOneShot(ServiceLocator.Instance.SoundManager.GrenadeExplosion);


        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                if(objectInRange.gameObject.GetComponent<Enemy>().isDead == false)
                {
                    objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
                }                
            }
        }
    }
}
