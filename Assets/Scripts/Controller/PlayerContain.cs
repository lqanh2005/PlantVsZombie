using UnityEngine;

public class PlayerContain : MonoBehaviour
{
    public int playerHealth;
    public GridController grid;
    public PlantSpawner plantSpawner;
    public ZombieSpawner zombieSpawner;
    public SunManager sunManager;
    public LevelController levelController;
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
