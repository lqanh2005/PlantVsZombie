using UnityEngine;

public class PlayerContain : MonoBehaviour
{
    public GridController grid;
    public PlantSpawner plantSpawner;
    public ZombieSpawner zombieSpawner;
    public SunManager sunManager;
    public LevelController levelController;
    public SkySunSpawner skySunSpawner;
    public void Init()
    {
        grid.Init();
        if (sunManager != null)
            sunManager.Init();
        if (skySunSpawner != null)
            skySunSpawner.Init();
        plantSpawner.Init();
        if (zombieSpawner != null)
            zombieSpawner.Init();
        if (levelController != null)
            levelController.Init();
    }
}
