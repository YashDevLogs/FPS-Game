using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "ScriptableObjects/Ammo", order = 1)]
public class Ammo : ScriptableObject
{
    public int ammoAmount = 200;
    public AmmoType ammoType;

    public enum AmmoType
    {
        RifleAmmo,
        PistolAmmo
    }
}
