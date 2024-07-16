using UnityEngine;
using static Weapon;


public class HUDManager 
{
    public void Update()
    {
        Weapon activeWeapon = ServiceLocator.Instance.WeaponManager.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            ServiceLocator.Instance.GlobalReference.magzineAmmoUI.text = $"{activeWeapon.BulletsLeft / activeWeapon.BulletsPerBurst}";
            ServiceLocator.Instance.GlobalReference.totalAmmoUI.text = $"{ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(activeWeapon.ThisWeaponModel)}";

            Weapon.WeaponEnum model = activeWeapon.ThisWeaponModel;
            ServiceLocator.Instance.GlobalReference.ammoTypeUI.sprite = GetAmmoSprite(model);

            ServiceLocator.Instance.GlobalReference.activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                ServiceLocator.Instance.GlobalReference.unActiceWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.ThisWeaponModel);

            }
        }
        else
        {
            ServiceLocator.Instance.GlobalReference.magzineAmmoUI.text = "";
            ServiceLocator.Instance.GlobalReference.totalAmmoUI.text = "";

            ServiceLocator.Instance.GlobalReference.ammoTypeUI.sprite = ServiceLocator.Instance.GlobalReference.emptySlot;

            ServiceLocator.Instance.GlobalReference.activeWeaponUI.sprite = ServiceLocator.Instance.GlobalReference.emptySlot;
            ServiceLocator.Instance.GlobalReference.unActiceWeaponUI.sprite= ServiceLocator.Instance.GlobalReference.emptySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in ServiceLocator.Instance.GlobalReference.weaponSlots)
        {
            if (weaponSlot != ServiceLocator.Instance.WeaponManager.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }return null;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponEnum model)
    {
        switch (model)
        {
            case WeaponEnum.Pistol:
                return Resources.Load<GameObject>("Pistol_Weapon").GetComponent<SpriteRenderer>().sprite;
            case WeaponEnum.Ak47:
                return Resources.Load<GameObject>("AK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;

        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponEnum model)
    {
        switch (model)
        {
            case WeaponEnum.Pistol:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case WeaponEnum.Ak47:
                return Resources.Load<GameObject>("Rfile_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;

        }
    }

    internal void UpdateThrowable(Throwable.ThrowableType throwable)
    {
        switch(throwable)
        {
            case Throwable.ThrowableType.Grenade:
                ServiceLocator.Instance.GlobalReference.lethalAmountUI.text = $"{ServiceLocator.Instance.WeaponManager.Grenades}";
                ServiceLocator.Instance.GlobalReference.lethalUI.sprite = Resources.Load<GameObject>("Frag").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
