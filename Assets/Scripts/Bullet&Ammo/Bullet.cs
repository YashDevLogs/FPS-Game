using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage;
    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        if (ObjectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit" + ObjectWeHit.gameObject.name + "!");
            CreateBulletImpactEffect(ObjectWeHit);
            Destroy(gameObject);
        }
        if (ObjectWeHit.gameObject.CompareTag("Wall"))
        {
            print("Hit a Wall!");
            CreateBulletImpactEffect(ObjectWeHit);
            Destroy(gameObject);
        }

        if (ObjectWeHit.gameObject.CompareTag("Destructable"))
        {
            print("Hit a Destructable!");
            ObjectWeHit.gameObject.GetComponent<Destructables>().Destruct();
        }

        if (ObjectWeHit.gameObject.CompareTag("Enemy"))
        {
            if(ObjectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                ObjectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(BulletDamage);
            }           
            CreateBloodSprayEffect(ObjectWeHit);
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision ObjectWeHit)
    {
        ContactPoint contact = ObjectWeHit.contacts[0];

        GameObject BloodSprayPrefab = Instantiate(
            GlobalReference.Instance.BloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );


        BloodSprayPrefab.transform.SetParent(ObjectWeHit.gameObject.transform);
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
