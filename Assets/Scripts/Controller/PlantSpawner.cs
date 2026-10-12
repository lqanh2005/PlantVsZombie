using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    private PlantData selectedPlantData;
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
        if (card == null || !card.IsSelectable || card.Data == null)
            return;

        SelectPlant(card.Data);
        selectedCard = card;
        selectedCard.SetSelected(true);
    }

    public void SelectPlant(PlantData data)
    {
        SetSelectedCard(null);
        selectedPlantData = data;
    }

    private void ClearSelection()
    {
        SetSelectedCard(null);
        selectedPlantData = null;
    }

    private void SetSelectedCard(PlantCard card)
    {
        if (selectedCard != null)
            selectedCard.SetSelected(false);
        selectedCard = card;
    }

    private static bool IsPlaying()
    {
        return GamePlayController.Instance.stateGame == StateGame.Playing;
    }

    public void FreeCell(int row, int col)
    {
        if (occupied == null || row < 0 || col < 0 || row >= occupied.GetLength(0) || col >= occupied.GetLength(1))
            return;
        occupied[row, col] = false;
    }

    private void Update()
    {
        if (!IsPlaying())
            return;

        if (selectedPlantData == null)
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

        if (!TryPlantAtScreenPosition(Mouse.current.position.ReadValue(), selectedPlantData))
            return;

        if (selectedCard != null)
            selectedCard.StartCooldown();
        ClearSelection();
    }

    public bool TryPlantAtScreenPosition(Vector2 screenPosition, PlantData plantData)
    {
        if (!IsPlaying() || plantData == null || plantData.plantPrefab == null)
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
        if (sunManager != null && !sunManager.CanAfford(plantData.sunCost))
            return false;

        GameObject plantObject = SimplePool.Spawn(plantData.plantPrefab, grid.GetCellCenterWorld(row, col) + spawnOffset, Quaternion.identity);
        PlantBase plant = plantObject.GetComponent<PlantBase>();
        if (plant == null)
        {
            SimplePool.Despawn(plantObject);
            return false;
        }

        plant.SetCell(row, col);
        plant.Init(plantData);
        if (!plant.isAlive)
            return false;

        if (sunManager != null)
            sunManager.TrySpend(plantData.sunCost);

        occupied[row, col] = true;
        return true;
    }
}
