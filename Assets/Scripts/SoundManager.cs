using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : GenericMonoSingleton<SoundManager>
{
    public AudioSource ShootingChannel;


    public AudioSource PistolReloading;
    public AudioSource AK47Reloading;

    public AudioClip AK47Shot;
    public AudioClip PistolShot;

    public AudioSource EmptyMagzine;


    public void PlayShootingSound(WeaponEnum weapon)
    {
        switch (weapon)
        {
            case WeaponEnum.Pistol:
                ShootingChannel.PlayOneShot(PistolShot);
                break;

            case WeaponEnum.Ak47:
                ShootingChannel.PlayOneShot(AK47Shot);
                break;
                
        }
    }

    public void PlayReloadSound(WeaponEnum weapon) 
    {
        switch (weapon)
        {
            case WeaponEnum.Pistol:
                PistolReloading.Play();
                break;

            case WeaponEnum.Ak47:
                AK47Reloading.Play();
                break;

        }
    }
}


