using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static Weapon;

public class WeaponManager : GenericMonoSingleton<WeaponManager>
{

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;

    [Header("Throwable")]
    public int grenades = 0;
    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;



    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot) 
            { 
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if(Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if(forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if(Input.GetKeyUp(KeyCode.G)) { 
        
            if(grenades > 0)
            {
                ThrowLethal();
                forceMultiplier = 0f;
            }
        }
    }




    #region || ----Weapon---- ||
    public void PickUpWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon); 
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);

        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapon.SpawnPosition.x, weapon.SpawnPosition.y, weapon.SpawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.SpawnRotation.x, weapon.SpawnRotation.y, weapon.SpawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
       if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    #endregion


    #region || ---- AmmoBox ---- ||
    internal void PickUpAmmoBox(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponEnum ThisWeaponModel)
    {
        switch(ThisWeaponModel)
        {
            case Weapon.WeaponEnum.Pistol:
                totalPistolAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponEnum.Ak47:
                totalRifleAmmo -= bulletsToDecrease;
                break;


        }
    }

    public int CheckAmmoLeft(WeaponEnum thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponEnum.Pistol:
                return totalPistolAmmo;

            case WeaponEnum.Ak47:
                return totalRifleAmmo;

            default:
                return 0;
        }
    }
    #endregion


    #region || ---- Throwable ---- ||
    internal void PickUpThrowable(Throwable throwable)
    {
        switch(throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickUpGrenade();
                break; 
        }
    }

    private void PickUpGrenade()
    {
        grenades += 1;
        HUDManager.Instance.UpdateThrowable(Throwable.ThrowableType.Grenade);


    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse) ;
        throwable.GetComponent<Throwable>().hasBeenThrown = true ;
        grenades -= 1;

        HUDManager.Instance.UpdateThrowable (Throwable.ThrowableType.Grenade);

    }

    #endregion
}
