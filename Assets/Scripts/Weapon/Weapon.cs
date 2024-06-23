using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Activ Weapon")]
    public bool isActiveWeapon;

    [Header("ADS")]
    public bool isADS;

    [Header("Shooting")]
    public bool IsShooting, ReadyToShoot;
    bool AllowReset = true;
    public float ShootingDelay = 2f;

    [Header("Burst")]
    public int BulletsPerBurst = 4;
    public int BurstBulletsLeft;

    [Header("Spread Intensity")]
    public float SpreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public Bullet BulletPrefab;
    public Transform BulletSpawn;
    public float BulletVelocity = 30f ;
    public float BulletPrefabLifespan = 3f;
    public float WeaponDamage;

    [Header("Effects")]
    public GameObject MuzzleEffect;
    public GameObject CartridgeEjectEffecst;
    internal Animator animator;

    [Header("Reload")]
    public float ReloadTime;
    public int MagzineSize, BulletsLeft;
    public bool IsReloading;

    [Header("Spawn Position")]
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;

    public ObjectPool<Bullet> bulletPool;

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

        SpreadIntensity = hipSpreadIntensity;

        bulletPool = new ObjectPool<Bullet>(BulletPrefab, 15);
    }


    void Update()
    {
        if (isActiveWeapon)
        {
            if (Input.GetMouseButtonDown(1))
            {
                
                EnterADS();
            }


            if (Input.GetMouseButtonUp(1))
            {
                
                ExitADS();
            }

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

            if (Input.GetKeyDown(KeyCode.R) && BulletsLeft < MagzineSize && !IsReloading && WeaponManager.Instance.CheckAmmoLeft(ThisWeaponModel) > 0)
            {
                Reload();
            }
            if (ReadyToShoot && !IsShooting && !IsReloading && BulletsLeft == 0 && WeaponManager.Instance.CheckAmmoLeft(ThisWeaponModel) > 0)
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


    private void EnterADS()
    {
        Camera.main.fieldOfView = 40;
        animator.SetTrigger("ADS_enter");
        isADS = true;
        SpreadIntensity = adsSpreadIntensity;
    }

    private void ExitADS()
    {
        Camera.main.fieldOfView = 60;
        animator.SetTrigger("ADS_exit");
        isADS = false;
    }


    private void FireWeapon()
    {
        BulletsLeft--;
        MuzzleEffect.GetComponent<ParticleSystem>().Play();

        SoundManager.Instance.PlayShootingSound(ThisWeaponModel);

        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
       
        CartridgeEjectEffecst.GetComponent<ParticleSystem>().Play();

        ReadyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        Bullet bullet = bulletPool.Get();
        bullet.transform.position = BulletSpawn.position;
        bullet.transform.forward = shootingDirection;
        bullet.BulletDamage = WeaponDamage;
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * BulletVelocity;
        bullet.SetPool(bulletPool);

        if (AllowReset)
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
       if(WeaponManager.Instance.CheckAmmoLeft(ThisWeaponModel) > MagzineSize)
        {
            BulletsLeft = MagzineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(BulletsLeft, ThisWeaponModel);
        }
        else 
        {
                BulletsLeft = WeaponManager.Instance.CheckAmmoLeft(ThisWeaponModel);
                WeaponManager.Instance.DecreaseTotalAmmo(BulletsLeft, ThisWeaponModel);         
        }

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

        float z = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);
        float y = UnityEngine.Random.Range(-SpreadIntensity, SpreadIntensity);

        return direction + new Vector3(0, y, z);
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
