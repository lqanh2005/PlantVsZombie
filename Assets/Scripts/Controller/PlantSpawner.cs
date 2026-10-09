using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlantSpawner : MonoBehaviour
{
    public GameObject selectedPlantPool;
    private bool[,] occupied = new bool[5, 9];

    public void SelectPlant(GameObject plantPrefab)
    {
        selectedPlantPool = plantPrefab;
    }
    private void Update()
    {
        if (selectedPlantPool == null)
            return;

        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (TryPlantAtScreenPosition(
            Mouse.current.position.ReadValue(),
            selectedPlantPool))
        {
            selectedPlantPool = null;
        }
    }

    public bool TryPlantAtScreenPosition(
        Vector2 screenPosition,
        GameObject plantPrefab)
    {
        if (plantPrefab == null)
            return false;

        Camera cam = Camera.main;
        if (cam == null)
            return false;

        var grid = GamePlayController.Instance.playerContain.grid;

        Ray ray = cam.ScreenPointToRay(screenPosition);

        Plane ground = new Plane(
            grid.transform.up,
            grid.transform.position
        );

        if (!ground.Raycast(ray, out float distance))
            return false;

        Vector3 hitPoint = ray.GetPoint(distance);

        if (!grid.TryGetTile(hitPoint, out int row, out int col))
            return false;

        if (row < 0 || row >= 5 || col < 0 || col >= 9)
            return false;

        if (occupied[row, col])
            return false;

        Vector3 spawnPosition =
            grid.GetCellCenterWorld(row, col);

        GameObject plant = SimplePool.Spawn(
            plantPrefab,
            spawnPosition,
            Quaternion.identity
        );
        plant.GetComponent<PlantBase>().Init();

        if (plant == null)
            return false;

        occupied[row, col] = true;
        return true;
    }

}
