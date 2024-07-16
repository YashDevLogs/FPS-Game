using UnityEngine;
using static Weapon;

public class WeaponManager 
{
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    private int totalPistolAmmo = 0;
    private int totalRifleAmmo = 0;

    [Header("Throwable")]
    private int grenades = 0;
    private float throwForce = 10f;
    private float forceMultiplier = 0f;
    private float forceMultiplierLimit = 2f;
    public int Grenades => grenades; // reference for HUD manager


    public void Initialize()
    {
        activeWeaponSlot = ServiceLocator.Instance.GlobalReference.weaponSlots[0];
    }

    public void Update()
    {
        foreach(GameObject weaponSlot in ServiceLocator.Instance.GlobalReference.weaponSlots)
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

        weapon.IsActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
       if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().IsActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        // Switching between 2 weapon slots 
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.IsActiveWeapon = false;
        }

        activeWeaponSlot = ServiceLocator.Instance.GlobalReference.weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.IsActiveWeapon = true;
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
        ServiceLocator.Instance.HUDManager.UpdateThrowable(Throwable.ThrowableType.Grenade);
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = ServiceLocator.Instance.GlobalReference.grenadePrefab;

        GameObject throwable = GameObject.Instantiate(lethalPrefab, ServiceLocator.Instance.GlobalReference.throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().HasBeenThrown = true;
        grenades -= 1;

        ServiceLocator.Instance.HUDManager.UpdateThrowable (Throwable.ThrowableType.Grenade);
    }
    #endregion
}
