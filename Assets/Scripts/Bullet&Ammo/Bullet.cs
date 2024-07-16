using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage; // reference for Weapon Manager

    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        Enemy enemy = ObjectWeHit.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            HandleEnemyCollision(ObjectWeHit, enemy);
        }
        else
        {
            HandleGeneralCollision(ObjectWeHit);
        }

        Destroy(gameObject);
    }

    private void HandleEnemyCollision(Collision ObjectWeHit, Enemy enemy)
    {
        if (!enemy.isDead)
        {
            enemy.TakeDamage(BulletDamage);
        }
        CreateBloodSprayEffect(ObjectWeHit);
    }

    private void HandleGeneralCollision(Collision ObjectWeHit)
    {
        CreateBulletImpactEffect(ObjectWeHit);
    }

    private void CreateBloodSprayEffect(Collision ObjectWeHit)
    {
        ContactPoint contact = ObjectWeHit.contacts[0];

        GameObject BloodSprayPrefab = Instantiate(
            ServiceLocator.Instance.GlobalReference.BloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        BloodSprayPrefab.transform.SetParent(ObjectWeHit.gameObject.transform);
    }

    public void CreateBulletImpactEffect(Collision ObjectWeHit)
    {
        ContactPoint contact = ObjectWeHit.contacts[0];

        GameObject hole = Instantiate(
            ServiceLocator.Instance.GlobalReference.BulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(ObjectWeHit.gameObject.transform);
    }
}
