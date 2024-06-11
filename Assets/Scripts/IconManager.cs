using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IconManager : MonoBehaviour
{
    public static Sprite Pistol_Weapon;
    public static Sprite AK47_Weapon;
    public static Sprite Pistol_Ammo;
    public static Sprite Rifle_Ammo;

    private void Awake()
    {

        Pistol_Weapon = Resources.Load<GameObject>("Pistol_Weapon").GetComponent<SpriteRenderer>().sprite;
        AK47_Weapon = Resources.Load<GameObject>("AK47_Weapon").GetComponent<SpriteRenderer>().sprite;
        Pistol_Ammo = Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
        Rifle_Ammo = Resources.Load<GameObject>("Rfile_Ammo").GetComponent<SpriteRenderer>().sprite;
    }
}