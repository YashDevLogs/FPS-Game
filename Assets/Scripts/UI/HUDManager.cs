using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;

public class HUDManager 
{
    private Image bloodScreenImage;
    private float bloodScreenTimer;
    private float bloodScreenDuration = 1.5f;
    private bool showBloodScreenEffect;
    [SerializeField] private TextMeshProUGUI healthUI;
    public TextMeshProUGUI HeathUI => healthUI;

    public void Init()
    {
        bloodScreenImage = ServiceLocator.Instance.GlobalReference.BloodScreenOverlay.GetComponentInChildren<Image>();
    }

    public void Update()
    {
        UpdateBloodScreenEffect();

    }

    public void FixedUpdate()
    {
        GetActiveWeapon();
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in ServiceLocator.Instance.GlobalReference.WeaponSlots)
        {
            if (weaponSlot != ServiceLocator.Instance.WeaponManager.ActiveWeaponSlot)
            {
                return weaponSlot;
            }
        }return null;
    }

    private void GetActiveWeapon()
    {
        Weapon activeWeapon = ServiceLocator.Instance.WeaponManager.ActiveWeaponSlot.GetComponentInChildren<Weapon>();// highlights the active weapon player which has selected as primary 
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();// moves the unactive weapon below the active weapon

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
            ServiceLocator.Instance.GlobalReference.UnActiceWeaponUI.sprite = ServiceLocator.Instance.GlobalReference.EmptySlot;
        }
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

    public void StartBloodScreenEffect()
    {
        if (!ServiceLocator.Instance.GlobalReference.BloodScreenOverlay.activeInHierarchy)
        {
            ServiceLocator.Instance.GlobalReference.BloodScreenOverlay.SetActive(true);
        }

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = bloodScreenImage.color;
        startColor.a = 1f;
        bloodScreenImage.color = startColor;

        bloodScreenTimer = bloodScreenDuration;
        showBloodScreenEffect = true;
    }

    public void UpdateBloodScreenEffect()
    {
        if (showBloodScreenEffect)
        {
            bloodScreenTimer -= Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, bloodScreenTimer / bloodScreenDuration);
            Color newColor = bloodScreenImage.color;
            newColor.a = alpha;
            bloodScreenImage.color = newColor;

            if (bloodScreenTimer <= 0)
            {
                showBloodScreenEffect = false;
                ServiceLocator.Instance.GlobalReference.BloodScreenOverlay.SetActive(false);
            }
        }
    }
}
