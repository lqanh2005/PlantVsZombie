using UnityEngine;

public class PlayerContain : MonoBehaviour
{
    public GridController grid;
    public PlantSpawner plantSpawner;
    public void Init()
    {
        grid.Init();
        //plantSpawner.Init();
    }
}
