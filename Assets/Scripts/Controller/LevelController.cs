using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<ZombieBase> currentZombies = new List<ZombieBase>();

    private LevelData levelData;
    private int currentWaveIndex;
    private int spawnedInWave;
    private float timer;
    private bool isWaitingWave;
    private bool isRunning;

    public LevelData LevelData => levelData;
    public int CurrentWave => currentWaveIndex + 1;
    public int TotalWaves => levelData != null ? levelData.waves.Count : 0;
    public bool IsFinished => levelData != null && !isRunning && currentZombies.Count == 0;

    public void Init()
    {
        currentZombies.Clear();
        currentWaveIndex = 0;
        spawnedInWave = 0;

        levelData = LevelDataList.Instance != null
            ? LevelDataList.Instance.GetLevelData(UseProfile.CurrentLevel)
            : null;

        isRunning = levelData != null && levelData.waves.Count > 0;
        if (isRunning)
            BeginWaitWave();
    }

    private void Update()
    {
        currentZombies.RemoveAll(z => z == null || !z.isAlive);

        if (!isRunning)
            return;

        if (GamePlayController.Instance.stateGame == StateGame.Lose)
            return;

        WaveData wave = levelData.waves[currentWaveIndex];
        timer -= Time.deltaTime;

        if (isWaitingWave)
        {
            bool cleared = wave.startWhenCleared && currentWaveIndex > 0 && currentZombies.Count == 0;
            if (timer > 0f && !cleared)
                return;

            isWaitingWave = false;
            timer = 0f;
        }

        if (timer > 0f)
            return;

        int count = Mathf.Max(1, wave.spawnPerTick);
        for (int i = 0; i < count && spawnedInWave < wave.zombieCount; i++)
        {
            SpawnRandomZombie(wave);
            spawnedInWave++;
        }

        timer = wave.spawnInterval;

        if (spawnedInWave >= wave.zombieCount)
            NextWave();
    }

    private void NextWave()
    {
        currentWaveIndex++;
        spawnedInWave = 0;

        if (currentWaveIndex >= levelData.waves.Count)
        {
            currentWaveIndex = levelData.waves.Count - 1;
            isRunning = false;
            return;
        }

        BeginWaitWave();
    }

    private void BeginWaitWave()
    {
        isWaitingWave = true;
        timer = levelData.waves[currentWaveIndex].startDelay;
    }

    private void SpawnRandomZombie(WaveData wave)
    {
        ZombieSpawnRate rate = PickZombie(wave.zombies);
        if (rate == null)
            return;

        EnemyData enemyData = EnemyDataBase.Instance.GetEnemyData(rate.enemyType, rate.enemyId);
        if (enemyData == null || enemyData.enemyPrefab == null)
            return;

        int row = wave.rows.Count > 0 ? wave.rows[Random.Range(0, wave.rows.Count)] : -1;

        ZombieBase zombie = GamePlayController.Instance.playerContain.zombieSpawner
            .SpawnZombie(enemyData.enemyPrefab, row);

        if (zombie != null && zombie.isAlive)
            currentZombies.Add(zombie);
    }

    private ZombieSpawnRate PickZombie(List<ZombieSpawnRate> zombies)
    {
        float totalWeight = 0f;
        foreach (ZombieSpawnRate rate in zombies)
        {
            if (rate != null && rate.weight > 0f)
                totalWeight += rate.weight;
        }

        if (totalWeight <= 0f)
            return null;

        float roll = Random.Range(0f, totalWeight);
        foreach (ZombieSpawnRate rate in zombies)
        {
            if (rate == null || rate.weight <= 0f)
                continue;

            roll -= rate.weight;
            if (roll < 0f)
                return rate;
        }

        return zombies[zombies.Count - 1];
    }
}
