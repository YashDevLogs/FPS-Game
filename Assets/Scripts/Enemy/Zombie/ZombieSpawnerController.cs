using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawnerController : MonoBehaviour
{
    private int InitialZombiesPerWave = 5;
    private int CurrentZombiesPerWave;

    [SerializeField] private float SpawnDelay = 0.5f;

    [SerializeField] private int CurrentWave = 0;

    [SerializeField] private float waveCooldown = 10f;

    [SerializeField] private bool InCooldown;
    [SerializeField] private float CooldownTimer = 0f;

    [SerializeField] private List<Enemy> CurrentZombiesAlive;

    [SerializeField] private Enemy ZombiePrefab;

    [SerializeField] private TextMeshProUGUI WaveOverUI;
    [SerializeField] private TextMeshProUGUI CountdownTimerUI;
    [SerializeField] private TextMeshProUGUI CurrentWaveUI;

    [SerializeField] private ObjectPool<Enemy> zombiePool;

    private void Start()
    {
        CurrentZombiesPerWave = InitialZombiesPerWave;
        zombiePool = new ObjectPool<Enemy>(ZombiePrefab, 70);
        ServiceLocator.Instance.GlobalReference.WaveNumber = CurrentWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        CurrentZombiesAlive.Clear();
        CurrentWave++;
        ServiceLocator.Instance.GlobalReference.WaveNumber = CurrentWave;
        CurrentWaveUI.text = "WAVE: " + CurrentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < CurrentZombiesPerWave; i++)
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
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
                StartCoroutine(ReturnZombieToPoolAfterDelay(zombie, 2.5f));
            }
        }

        foreach (Enemy zombie in zombiesToRemove)
        {
            CurrentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        if (CurrentZombiesAlive.Count == 0 && !InCooldown)
        {
            StartCoroutine(WaveCooldown());
        }

        if (InCooldown)
        {
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer <= 0)
            {
                InCooldown = false;
                WaveOverUI.gameObject.SetActive(false);
                CurrentZombiesPerWave *= 2;
                StartNextWave();
            }
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
        CooldownTimer = waveCooldown;

        yield return new WaitForSeconds(waveCooldown);

        // This section is moved to Update to avoid immediate next wave start.
    }

    private IEnumerator ReturnZombieToPoolAfterDelay(Enemy zombie, float delay)
    {
        yield return new WaitForSeconds(delay);
        zombiePool.ReturnToPool(zombie);
        zombie.isDead = false;
    }
}
