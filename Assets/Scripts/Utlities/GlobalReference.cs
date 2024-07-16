using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalReference : MonoBehaviour
{
    public GameObject BulletImpactEffectPrefab;
    public GameObject grenadeExplosionEffect;

    public GameObject BloodSprayEffect;

    [Header("WeaponManager")]
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public List<GameObject> weaponSlots;

    [Header("HUDManager")]

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

    public int WaveNumber;

    public Transform PlayerTransfrom;
    public List<Transform> WayPoints;

}
