using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

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

    public GameObject MuzzleEffect;
    public GameObject CartridgeEjectEffecst;
    internal Animator animator;

    public float ReloadTime;
    public int MagzineSize, BulletsLeft;
    public bool IsReloading;

    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;

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
        animator = GetComponent<Animator>();
        BulletsLeft = MagzineSize;
    }


    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (BulletsLeft == 0 && IsShooting)
            {
                SoundManager.Instance.EmptyMagzine.Play();
            }

            if (CurrentShootingMode == ShootingMode.Auto)
            {
                IsShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (CurrentShootingMode == ShootingMode.Single || CurrentShootingMode == ShootingMode.Burst)
            {
                IsShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < MagzineSize && !IsReloading)
            {
                Reload();
            }
            if (ReadyToShoot && !IsShooting && !IsReloading && BulletsLeft <= 0)
            {
                Reload();
            }


            if (ReadyToShoot && IsShooting && BulletsLeft > 0)
            {
                BurstBulletsLeft = BulletsPerBurst;
                FireWeapon();

            }
        }
    }



    private void FireWeapon()
    {
        BulletsLeft--;
        MuzzleEffect.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.PlayShootingSound(ThisWeaponModel);
        animator.SetTrigger("RECOIL");
        CartridgeEjectEffecst.GetComponent<ParticleSystem>().Play();

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


    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(ThisWeaponModel);
        IsReloading = true;
        animator.SetTrigger("IsReloading");
        Invoke("ReloadComplete", ReloadTime);
    }

    private void ReloadComplete()
    {
        BulletsLeft = MagzineSize;
        IsReloading = false;
    }
    private void ResetShot()
    {
        ReadyToShoot = true;
        AllowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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

    public enum WeaponEnum
    {
        Pistol,
        Ak47
    }
    public WeaponEnum ThisWeaponModel;
}
