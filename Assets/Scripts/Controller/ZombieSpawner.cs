using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieSpawnEntry
{
    public GameObject zombiePrefab;
    public float delay = 3f;
    [Tooltip("-1 = hàng ngẫu nhiên")]
    public int row = -1;
}

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private List<ZombieSpawnEntry> spawnEntries = new List<ZombieSpawnEntry>();
    [SerializeField] private float spawnOffsetX = 2f;
    [SerializeField] private bool loop;

    private readonly List<ZombieBase> activeZombies = new List<ZombieBase>();
    private int currentIndex;
    private float spawnTimer;
    private bool isRunning;

    public IReadOnlyList<ZombieBase> ActiveZombies => activeZombies;

    public void Init()
    {
        activeZombies.Clear();
        currentIndex = 0;
        isRunning = spawnEntries.Count > 0;
        if (isRunning)
            spawnTimer = spawnEntries[0].delay;
    }

    private void Update()
    {
        activeZombies.RemoveAll(z => z == null || !z.isAlive);

        if (!isRunning)
            return;

        if (GamePlayController.Instance.stateGame == StateGame.Lose)
            return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f)
            return;

        ZombieSpawnEntry entry = spawnEntries[currentIndex];
        SpawnZombie(entry.zombiePrefab, entry.row);

        currentIndex++;
        if (currentIndex >= spawnEntries.Count)
        {
            if (!loop)
            {
                isRunning = false;
                return;
            }
            currentIndex = 0;
        }
        spawnTimer = spawnEntries[currentIndex].delay;
    }

    public ZombieBase SpawnZombie(GameObject zombiePrefab, int row = -1)
    {
        if (zombiePrefab == null)
            return null;

        GridController grid = GamePlayController.Instance.playerContain.grid;
        if (row < 0 || row >= grid.Rows)
            row = Random.Range(0, grid.Rows);

        Vector3 spawnPosition = grid.GetCellCenterWorld(row, grid.Cols - 1) + Vector3.right * spawnOffsetX;
        GameObject zombieObject = SimplePool.Spawn(zombiePrefab, spawnPosition, zombiePrefab.transform.rotation);

        ZombieBase zombie = zombieObject.GetComponent<ZombieBase>();
        if (zombie == null)
            return null;

        zombie.Init();
        if (zombie.isAlive)
            activeZombies.Add(zombie);
        return zombie;
    }

    public bool IsFinished => !isRunning && activeZombies.Count == 0;
}
