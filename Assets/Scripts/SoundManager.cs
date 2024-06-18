using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : GenericMonoSingleton<SoundManager>
{
    [Header("Audio Source Channel")]
    public AudioSource ShootingChannel;
    public AudioSource ThrowableChannel;
    public AudioSource ZombieChannel;
    public AudioSource ZombieChannel2;

    [Header("Pistol Sound")]
    public AudioSource PistolReloading;
    public AudioClip PistolShot;
    public AudioSource EmptyMagzine;

    [Header("Ak-47 Sound")] 
    public AudioSource AK47Reloading;
    public AudioClip AK47Shot;

    [Header("Grenade Sound")]
    public AudioClip GrenadeExplosion;

    [Header("Zombie Sound")]
    public AudioClip ZombieWalking;
    public AudioClip ZombieChase;
    public AudioClip ZombieAttack;
    public AudioClip ZombieHurt;
    public AudioClip ZombieDeath;

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


