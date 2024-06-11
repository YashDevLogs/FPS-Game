using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;


public class HUDManager : MonoBehaviour
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
    public Sprite greySlot;
    public GameObject middleAim;

    private void Update()
    {
        Weapon activeWeapon = GameManager.Instance.WeaponManager.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magzineAmmoUI.text = $"{activeWeapon.BulletsLeft / activeWeapon.BulletsPerBurst}";
            totalAmmoUI.text = $"{GameManager.Instance.WeaponManager.CheckAmmoLeft(activeWeapon.ThisWeaponModel)}";

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

        if(GameManager.Instance.WeaponManager.grenades <= 0)
        {

            lethalUI.sprite = greySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in GameManager.Instance.WeaponManager.weaponSlots)
        {
            if (weaponSlot != GameManager.Instance.WeaponManager.activeWeaponSlot)
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
                return IconManager.Pistol_Weapon;
            case WeaponEnum.Ak47:
                return IconManager.AK47_Weapon;
            default:
                return null;

        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponEnum model)
    {
        switch (model)
        {
            case WeaponEnum.Pistol:
                return IconManager.Pistol_Ammo;
            case WeaponEnum.Ak47:
                return IconManager.Rifle_Ammo;
            default:
                return null;

        }
    }

    internal void UpdateThrowable(Throwable.ThrowableType throwable)
    {
        switch(throwable)
        {
            case Throwable.ThrowableType.Grenade:
                lethalAmountUI.text = $"{GameManager.Instance.WeaponManager.grenades}";
                lethalUI.sprite = Resources.Load<GameObject>("Frag").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
