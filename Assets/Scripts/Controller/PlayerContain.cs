using UnityEngine;

public class PlayerContain : MonoBehaviour
{
    public GridController grid;
    public PlantSpawner plantSpawner;
    public ZombieSpawner zombieSpawner;
    public SunManager sunManager;
    public void Init()
    {
        grid.Init();
        if (sunManager != null)
            sunManager.Init();
        plantSpawner.Init();
        if (zombieSpawner != null)
            zombieSpawner.Init();
    }
}
