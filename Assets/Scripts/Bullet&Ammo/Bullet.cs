using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage;

    private ObjectPool<Bullet> pool;

    public void SetPool(ObjectPool<Bullet> pool)
    {
        this.pool = pool;
    }

    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        if (ObjectWeHit.gameObject.CompareTag("Target") ||
            ObjectWeHit.gameObject.CompareTag("Wall") ||
            ObjectWeHit.gameObject.CompareTag("Enemy"))
        {
            HandleCollision(ObjectWeHit);
            pool.ReturnToPool(this);
        }
    }

    private void HandleCollision(Collision ObjectWeHit)
    {
        if (ObjectWeHit.gameObject.CompareTag("Target"))
        {          
            CreateBulletImpactEffect(ObjectWeHit);
        }
        else if (ObjectWeHit.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(ObjectWeHit);
        }
        else if (ObjectWeHit.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = ObjectWeHit.gameObject.GetComponent<Enemy>();
            if (!enemy.isDead)
            {
                enemy.TakeDamage(BulletDamage);
            }
            CreateBloodSprayEffect(ObjectWeHit);
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
