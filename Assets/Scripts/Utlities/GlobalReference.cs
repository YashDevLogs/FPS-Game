using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalReference : MonoBehaviour
{
    [Header("Private References")]
    [SerializeField] private GameObject bulletImpactEffectPrefab;
    [SerializeField] private GameObject grenadeExplosionEffect;

    [SerializeField] private GameObject bloodSprayEffect;

    [Header("WeaponManager")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject throwableSpawn;
    [SerializeField] private List<GameObject> weaponSlots;

    [Header("HUDManager")]
    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI magzineAmmoUI;
    [SerializeField] private TextMeshProUGUI totalAmmoUI;
    [SerializeField] private Image ammoTypeUI;

    [Header("Weapon")]
    [SerializeField] private Image activeWeaponUI;
    [SerializeField] private Image unActiceWeaponUI;

    [Header("Throwables")]
    [SerializeField] private Image lethalUI;
    [SerializeField] private TextMeshProUGUI lethalAmountUI;

    [SerializeField] private Image tacticalUI;
    [SerializeField] private TextMeshProUGUI tacticalAmountUI;

    [SerializeField] private Sprite emptySlot;
    [SerializeField] private GameObject middleAim;

    [SerializeField] private Transform playerTransfrom;
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private GameObject bloodScreenOverlay;

    // Public references
    public GameObject BulletImpactEffectPrefab => bulletImpactEffectPrefab;
    public GameObject GrenadeExplosionEffect => grenadeExplosionEffect;
    public GameObject BloodSprayEffect => bloodSprayEffect;
    public GameObject GrenadePrefab => grenadePrefab;
    public GameObject ThrowableSpawn => throwableSpawn;
    public List<GameObject> WeaponSlots => weaponSlots;
    public TextMeshProUGUI MagzineAmmoUI => magzineAmmoUI;
    public TextMeshProUGUI TotalAmmoUI  => totalAmmoUI;
    public Image AmmoTypeUI => ammoTypeUI;
    public Image ActiveWeaponUI => activeWeaponUI;
    public Image UnActiceWeaponUI => unActiceWeaponUI;
    public Image LethalUI => lethalUI;
    public TextMeshProUGUI LethalAmountUI => lethalAmountUI;
    public Image TacticalUI => tacticalUI;
    public TextMeshProUGUI TacticalAmountUI => tacticalAmountUI;
    public Sprite EmptySlot => emptySlot;
    public GameObject MiddleAim => middleAim;
    public Transform PlayerTransfrom => playerTransfrom;
    public List<Transform> WayPoints => wayPoints;
    public int WaveNumber;
    public GameObject BloodScreenOverlay => bloodScreenOverlay;
}
