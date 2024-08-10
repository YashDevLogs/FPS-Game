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
            ServiceLocator.Instance.GlobalReference.MagzineAmmoUI.text = $"{activeWeapon.BulletsLeft / activeWeapon.BulletsPerBurst}";
            ServiceLocator.Instance.GlobalReference.TotalAmmoUI.text = $"{ServiceLocator.Instance.WeaponManager.CheckAmmoLeft(activeWeapon.ThisWeaponModel)}";

            Weapon.WeaponEnum model = activeWeapon.ThisWeaponModel;
            ServiceLocator.Instance.GlobalReference.AmmoTypeUI.sprite = GetAmmoSprite(model);

            ServiceLocator.Instance.GlobalReference.ActiveWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                ServiceLocator.Instance.GlobalReference.UnActiceWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.ThisWeaponModel);

            }
        }
        else
        {
            ServiceLocator.Instance.GlobalReference.MagzineAmmoUI.text = "";
            ServiceLocator.Instance.GlobalReference.TotalAmmoUI.text = "";

            ServiceLocator.Instance.GlobalReference.AmmoTypeUI.sprite = ServiceLocator.Instance.GlobalReference.EmptySlot;

            ServiceLocator.Instance.GlobalReference.ActiveWeaponUI.sprite = ServiceLocator.Instance.GlobalReference.EmptySlot;
            ServiceLocator.Instance.GlobalReference.UnActiceWeaponUI.sprite= ServiceLocator.Instance.GlobalReference.EmptySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in ServiceLocator.Instance.GlobalReference.WeaponSlots)
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
                ServiceLocator.Instance.GlobalReference.LethalAmountUI.text = $"{ServiceLocator.Instance.WeaponManager.Grenades}";
                ServiceLocator.Instance.GlobalReference.LethalUI.sprite = Resources.Load<GameObject>("Frag").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
