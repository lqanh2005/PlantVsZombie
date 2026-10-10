
using UnityEngine;

public class SelectedSeedGrid : MonoBehaviour
{
    [SerializeField] private SlotCardUI[] slots;
    [SerializeField] private PlantCardUI selectedCardPrefab;

    public void TrySelectPlant(PlantCardUI sourceCard)
    {
        if (sourceCard == null || sourceCard.IsSelected)
            return;

        SlotCardUI emptySlot = null;

        foreach (SlotCardUI slot in slots)
        {
            if (slot != null && !slot.IsOccupied)
            {
                emptySlot = slot;
                break;
            }
        }

        if (emptySlot == null)
            return;

        PlantData data = PlantDatabase.Instance.GetPlantData(
            sourceCard.plantType,
            sourceCard.plantId
        );

        // Spawn card mới tại vị trí card nguồn.
        PlantCardUI newCard = Instantiate(
            selectedCardPrefab,
            sourceCard.transform.position,
            Quaternion.identity,
            sourceCard.transform.parent
        );

        newCard.SetData(data);

        // Đánh dấu slot và card nguồn.
        emptySlot.SetOccupied(true);
        sourceCard.SetSelected(true);

        // Di chuyển card mới đến slot đích.
        newCard.MoveToSlot(emptySlot.GetComponent<RectTransform>());

        HomeController.Instance.selectedPlants.Add(
            new SelectedPlantData(sourceCard.plantId, sourceCard.plantType)
        );
    }
}
