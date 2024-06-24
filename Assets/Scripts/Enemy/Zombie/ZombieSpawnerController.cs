using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class ZombieSpawnerController : MonoBehaviour
{
    public int InitialZombiesPerWave = 5;
    public int CurrentZombiesPerWave;

    public float SpawnDelay = 0.5f;

    public int CurrentWave = 0;

    public float waveCooldown = 10f;

    public bool InCooldown;
    public float CooldownTimer = 0f;

    public List<Enemy> CurrentZombiesAlive;

    public Enemy ZombiePrefab;


    public TextMeshProUGUI WaveOverUI;
    public TextMeshProUGUI CountdownTimerUI;
    public TextMeshProUGUI CurrentWaveUI;


    public ObjectPool<Enemy> zombiePool;


    private void Start()
    {
        
        CurrentZombiesPerWave = InitialZombiesPerWave;

        zombiePool = new ObjectPool<Enemy>(ZombiePrefab, 20);

        StartNextWave();
    }

    private void StartNextWave()
    {
        CurrentZombiesAlive.Clear();

        CurrentWave++;

        CurrentWaveUI.text = "WAVE: " + CurrentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
       for(int i = 0; i < CurrentZombiesPerWave;  i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPos = transform.position + spawnOffset;

            var zombie = zombiePool.Get();
            zombie.transform.position = spawnPos;
            zombie.transform.rotation = Quaternion.identity;
            CurrentZombiesAlive.Add(zombie);


            yield return new WaitForSeconds(SpawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();

        foreach (Enemy zombie in CurrentZombiesAlive)
        {
            if(zombie.isDead)
            {
                zombiesToRemove.Add(zombie);

                zombiePool.ReturnToPool(zombie);

            }
        }

        foreach (Enemy zombie in zombiesToRemove)
        {
            CurrentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();


        if(CurrentZombiesAlive.Count == 0 && InCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (InCooldown)
        {
            CooldownTimer -= Time.deltaTime;
        }
        else
        {
            CooldownTimer = waveCooldown;
        }


        CountdownTimerUI.text = CooldownTimer.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        InCooldown = true;
        WaveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        InCooldown = false;
        WaveOverUI.gameObject.SetActive(false);

        CurrentZombiesPerWave *= 2;
        StartNextWave();
    }



}
