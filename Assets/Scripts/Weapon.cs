using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera PlayerCamera;

    public bool IsShooting, ReadyToShoot;
    bool AllowReset = true;
    public float ShootingDelay = 2f;

    public int BulletsPerBurst = 4;
    public int BurstBulletsLeft;

    public float SpreadIntensity;

    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public float BulletVelocity = 30f ;
    public float BulletPrefabLifespan = 3f;


    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode CurrentShootingMode;

    private void Awake()
    {
        ReadyToShoot = true;
        BurstBulletsLeft = BulletsPerBurst;
    }


    void Update()
    {
        if(CurrentShootingMode == ShootingMode.Auto) 
        {
            IsShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(CurrentShootingMode == ShootingMode.Single || CurrentShootingMode == ShootingMode.Burst) 
        {
            IsShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if(ReadyToShoot && IsShooting)
        {
            BurstBulletsLeft = BulletsPerBurst;
            FireWeapon();

        }
    }



    private void FireWeapon()
    {
        ReadyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection *  BulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, BulletPrefabLifespan));

        if(AllowReset)
        {
            Invoke("ResetShot", ShootingDelay);
            AllowReset = false;
        }

        if(CurrentShootingMode == ShootingMode.Burst && BurstBulletsLeft > 1)
        {
            BurstBulletsLeft--;
            Invoke("FireWeapon", ShootingDelay);
        }
    }

    private void ResetShot()
    {
        ReadyToShoot = true;
        AllowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - BulletSpawn.position;

        float x = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);
        float y = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifespan)
    {
        yield return new WaitForSeconds(bulletPrefabLifespan);
        Destroy(bullet);
    }
}
