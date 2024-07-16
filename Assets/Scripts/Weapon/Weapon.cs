using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Activ Weapon")]
    public bool IsActiveWeapon; // Needs to be modified in Weapon Manager if the weapon is Active or Inactive

    [Header("ADS")]
    [SerializeField] private bool isADS;

    [Header("Shooting")]
    [SerializeField] private bool isShooting, readyToShoot;
    [SerializeField] private bool allowReset = true;
    [SerializeField] private float shootingDelay = 2f;

    [Header("Burst")]
    [SerializeField] private int bulletsPerBurst = 4;
    [SerializeField] private int burstBulletsLeft;
    public int BulletsPerBurst => bulletsPerBurst; // Needs to be updated in HUD manager

    [Header("Spread Intensity")]
    [SerializeField] private float spreadIntensity;
    [SerializeField] private float hipSpreadIntensity;
    [SerializeField] private float adsSpreadIntensity;

    [Header("Bullet")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletVelocity = 30f;
    [SerializeField] private float bulletPrefabLifespan = 3f;
    [SerializeField] private float weaponDamage;

    [Header("Effects")]
    [SerializeField] private GameObject MuzzleEffect;
    [SerializeField] private GameObject CartridgeEjectEffecst;
    internal Animator animator; // Weapon Manager use to enable animator if its an active weapon

    [Header("Reload")]
    [SerializeField] private float ReloadTime;
    [SerializeField] private int MagzineSize, bulletsLeft;
    [SerializeField] private bool IsReloading;

    public int BulletsLeft => bulletsLeft; // Needs to be updated in HUD manager

    [Header("Spawn Position")]
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
        readyToShoot = true;
        burstBulletsLeft = BulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = MagzineSize;

        spreadIntensity = hipSpreadIntensity;

    }


    void Update()
    {
        if (IsActiveWeapon)
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
            if (bulletsLeft == 0 && isShooting)
            {
                ServiceLocator.Instance.SoundManager.EmptyMagzine.Play();
            }

            if (CurrentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (CurrentShootingMode == ShootingMode.Single || CurrentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < MagzineSize && !IsReloading && ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(ThisWeaponModel) > 0)
            {
                Reload();
            }
            if (readyToShoot && !isShooting && !IsReloading && bulletsLeft == 0 && ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(ThisWeaponModel) > 0)
            {
                Reload();
            }


            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = BulletsPerBurst;
                FireWeapon();

            }
        }
    }


    private void EnterADS()
    {
        Camera.main.fieldOfView = 40;
        animator.SetTrigger("ADS_enter");
        isADS = true;
        spreadIntensity = adsSpreadIntensity;
    }

    private void ExitADS()
    {
        Camera.main.fieldOfView = 60;
        animator.SetTrigger("ADS_exit");
        isADS = false;
    }


    private void FireWeapon()
    {
        bulletsLeft--;
        MuzzleEffect.GetComponent<ParticleSystem>().Play();

        ServiceLocator.Instance.SoundManager.PlayShootingSound(ThisWeaponModel);

        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }

        CartridgeEjectEffecst.GetComponent<ParticleSystem>().Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        Bullet bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.BulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifespan));


        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (CurrentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }


    private void Reload()
    {
        ServiceLocator.Instance.SoundManager.PlayReloadSound(ThisWeaponModel);
        IsReloading = true;
        animator.SetTrigger("IsReloading");
        Invoke("ReloadComplete", ReloadTime);
    }

    private void ReloadComplete()
    {
        if (ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(ThisWeaponModel) > MagzineSize)
        {
            bulletsLeft = MagzineSize;
            ServiceLocator.Instance.WeaponManager.DecreaseTotalAmmo(bulletsLeft, ThisWeaponModel);
        }
        else
        {
            bulletsLeft = ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(ThisWeaponModel);
            ServiceLocator.Instance.WeaponManager.DecreaseTotalAmmo(bulletsLeft, ThisWeaponModel);
        }

        IsReloading = false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }
    private IEnumerator DestroyBulletAfterTime(Bullet bullet, float bulletPrefabLifespan)
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