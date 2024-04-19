using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + "!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Hit a Wall!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
    }

    public void CreateBulletImpactEffect(Collision ObjectWeHit)
    {
        ContactPoint contact = ObjectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReference.Instance.BulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            ); 

        hole.transform.SetParent(ObjectWeHit.gameObject.transform);
    }

}
