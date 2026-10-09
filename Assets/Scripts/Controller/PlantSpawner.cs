using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlantSpawner : MonoBehaviour
{
    public GameObject selectedPlantPool;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    private int selectedSunCost;
    private PlantCard selectedCard;
    private bool[,] occupied;

    public void Init()
    {
        GridController grid = GamePlayController.Instance.playerContain.grid;
        occupied = new bool[grid.Rows, grid.Cols];
        ClearSelection();
    }

    public void ToggleSelect(PlantCard card)
    {
        if (card != null && card == selectedCard)
        {
            ClearSelection();
            return;
        }

        SelectPlant(card);
    }

    public void SelectPlant(PlantCard card)
    {
        if (card == null || !card.IsSelectable)
            return;

        SelectPlant(card.plantPrefab, card.SunCost);
        selectedCard = card;
        selectedCard.SetSelected(true);
    }

    public void SelectPlant(GameObject plantPrefab, int sunCost = 0)
    {
        SetSelectedCard(null);
        selectedPlantPool = plantPrefab;
        selectedSunCost = sunCost;
    }

    private void ClearSelection()
    {
        SetSelectedCard(null);
        selectedPlantPool = null;
        selectedSunCost = 0;
    }

    private void SetSelectedCard(PlantCard card)
    {
        if (selectedCard != null)
            selectedCard.SetSelected(false);
        selectedCard = card;
    }

    public void FreeCell(int row, int col)
    {
        if (occupied == null || row < 0 || col < 0 || row >= occupied.GetLength(0) || col >= occupied.GetLength(1))
            return;
        occupied[row, col] = false;
    }

    private void Update()
    {
        if (selectedPlantPool == null)
            return;

        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ClearSelection();
            return;
        }

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (!TryPlantAtScreenPosition(Mouse.current.position.ReadValue(), selectedPlantPool))
            return;

        if (selectedCard != null)
            selectedCard.StartCooldown();
        ClearSelection();
    }

    public bool TryPlantAtScreenPosition(Vector2 screenPosition, GameObject plantPrefab)
    {
        if (plantPrefab == null)
            return false;

        Camera cam = Camera.main;
        if (cam == null)
            return false;

        PlayerContain playerContain = GamePlayController.Instance.playerContain;
        GridController grid = playerContain.grid;
        if (occupied == null)
            Init();

        Ray ray = cam.ScreenPointToRay(screenPosition);
        Plane ground = new Plane(grid.transform.up, grid.transform.position);

        if (!ground.Raycast(ray, out float distance))
            return false;

        Vector3 hitPoint = ray.GetPoint(distance);

        if (!grid.TryGetTile(hitPoint, out int row, out int col))
            return false;

        if (occupied[row, col])
            return false;

        SunManager sunManager = playerContain.sunManager;
        if (sunManager != null && !sunManager.CanAfford(selectedSunCost))
            return false;

        GameObject plantObject = SimplePool.Spawn(plantPrefab, grid.GetCellCenterWorld(row, col) + spawnOffset, Quaternion.identity);
        PlantBase plant = plantObject.GetComponent<PlantBase>();
        if (plant == null)
        {
            SimplePool.Despawn(plantObject);
            return false;
        }

        plant.SetCell(row, col);
        plant.Init();
        if (!plant.isAlive)
            return false;

        if (sunManager != null)
            sunManager.TrySpend(selectedSunCost);

        occupied[row, col] = true;
        return true;
    }
}
