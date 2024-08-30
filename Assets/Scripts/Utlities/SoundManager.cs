using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Channels")]
    [SerializeField] private AudioSource PlayerChannel;
    [SerializeField] private AudioSource zombieChannel;
    [SerializeField] private AudioSource ZombieChannel2;
    [SerializeField] private AudioSource ShootingChannel;
    [SerializeField] private AudioSource SFXChannel;
    [SerializeField] private AudioSource ThrowableChannel;
    public AudioSource ZombieChannel => zombieChannel;

    [Header("Sounds")]
    public List<Sound> sounds;

    private Dictionary<string, AudioSource> channelDictionary;
    private Dictionary<string, Sound> soundDictionary;

    private void Awake()
    {
        // Initialize dictionaries
        channelDictionary = new Dictionary<string, AudioSource>
        {
            { "PlayerChannel", PlayerChannel },
            { "ZombieChannel", zombieChannel },
            { "ZombieChannel2", ZombieChannel2 },
            { "ShootingChannel", ShootingChannel },
            { "SFXChannel", SFXChannel },
            { "ThrowableChannel", ThrowableChannel }
        };

        soundDictionary = new Dictionary<string, Sound>();
        foreach (var sound in sounds)
        {
            soundDictionary[sound.soundName] = sound;
        }
    }

    public void Start()
    {
        PlaySound("PlayerEfforts", "PlayerChannel");
        PlaySound("CrowScreaming", "SFXChannel");
    }

    public void PlaySound(string soundName, string channelName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound) &&
            channelDictionary.TryGetValue(channelName, out AudioSource channel))
        {
            channel.clip = sound.clip;
            channel.loop = sound.loop;
            channel.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} or channel {channelName} not found in SoundManager.");
        }
    }

    public void PlayOneShot(string soundName, string channelName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound) &&
            channelDictionary.TryGetValue(channelName, out AudioSource channel))
        {
            channel.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} or channel {channelName} not found in SoundManager.");
        }
    }

    public void PlayShootingSound(WeaponEnum weapon)
    {
        switch (weapon)
        {
            case WeaponEnum.Pistol:
                PlayOneShot("PistolShot","ShootingChannel");
                break;

            case WeaponEnum.Ak47:
                PlayOneShot("AK47Shot", "ShootingChannel");
                break;
        }
    }

    public void PlayReloadSound(WeaponEnum weapon)
    {
        switch (weapon)
        {
            case WeaponEnum.Pistol:
                PlayOneShot("PistolReloading", "ShootingChannel");
                break;

            case WeaponEnum.Ak47:
                PlayOneShot("AK47Reloading", "ShootingChannel");
                break;
        }
    }


}
