using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private float spawnOffsetX = 2f;

    public void Init()
    {
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
        return zombie;
    }
}
