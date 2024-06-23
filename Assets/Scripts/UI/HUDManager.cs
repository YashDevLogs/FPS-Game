using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;


public class HUDManager : GenericMonoSingleton<HUDManager>
{
    [Header("Ammo")]
    public TextMeshProUGUI magzineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiceWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public GameObject middleAim;

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magzineAmmoUI.text = $"{activeWeapon.BulletsLeft / activeWeapon.BulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeft(activeWeapon.ThisWeaponModel)}";

            Weapon.WeaponEnum model = activeWeapon.ThisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiceWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.ThisWeaponModel);

            }
        }
        else
        {
            magzineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;  
            unActiceWeaponUI.sprite= emptySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
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
                lethalAmountUI.text = $"{WeaponManager.Instance.grenades}";
                lethalUI.sprite = Resources.Load<GameObject>("Frag").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
