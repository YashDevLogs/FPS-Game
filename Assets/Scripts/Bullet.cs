using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
