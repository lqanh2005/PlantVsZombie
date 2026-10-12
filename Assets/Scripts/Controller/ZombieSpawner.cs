using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private float spawnOffsetX = 2f;

    public void Init()
    {
    }

    public ZombieBase SpawnZombie(EnemyData enemyData, int row = -1)
    {
        if (enemyData == null || enemyData.enemyPrefab == null)
            return null;

        GameObject zombiePrefab = enemyData.enemyPrefab;

        GridController grid = GamePlayController.Instance.playerContain.grid;
        if (row < 0 || row >= grid.Rows)
            row = Random.Range(0, grid.Rows);

        Vector3 spawnPosition = grid.GetCellCenterWorld(row, grid.Cols - 1) + Vector3.right * spawnOffsetX;
        GameObject zombieObject = SimplePool.Spawn(zombiePrefab, spawnPosition, zombiePrefab.transform.rotation);

        ZombieBase zombie = zombieObject.GetComponent<ZombieBase>();
        if (zombie == null)
        {
            SimplePool.Despawn(zombieObject);
            return null;
        }

        zombie.Init(enemyData);
        return zombie;
    }
}
