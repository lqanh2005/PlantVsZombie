using UnityEngine;
using System.Collections.Generic;

public class PlantList : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private PlantCard cardPrefab;

    public List<PlantCard> plants = new List<PlantCard>();

    private readonly List<SlotCardUI> slots = new List<SlotCardUI>();

    public void Init()
    {
        CollectSlots();
        ClearCards();

        if (cardPrefab == null)
        {
            Debug.LogError("PlantList: Chưa gán cardPrefab!");
            return;
        }

        List<SelectedPlantData> selectedPlants = UseProfile.LoadSelectedPlants();
        if (selectedPlants.Count == 0)
            selectedPlants = GetDefaultPlants();

        int count = Mathf.Min(selectedPlants.Count, slots.Count);
        for (int i = 0; i < count; i++)
        {
            SlotCardUI slot = slots[i];
            PlantCard card = Instantiate(cardPrefab, slot.transform, false);

            RectTransform rect = (RectTransform)card.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;

            card.Init(selectedPlants[i].plantType, selectedPlants[i].plantId);
            slot.SetOccupied(true);
            plants.Add(card);
        }
    }

    private void CollectSlots()
    {
        slots.Clear();

        Transform container = slotContainer != null ? slotContainer : transform;
        foreach (Transform child in container)
        {
            SlotCardUI slot = child.GetComponent<SlotCardUI>();
            if (slot == null)
                slot = child.gameObject.AddComponent<SlotCardUI>();
            slot.SetOccupied(false);
            slots.Add(slot);
        }
    }

    private void ClearCards()
    {
        foreach (PlantCard card in plants)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        plants.Clear();
    }

    private List<SelectedPlantData> GetDefaultPlants()
    {
        List<SelectedPlantData> result = new List<SelectedPlantData>();
        foreach (PlantData data in PlantDatabase.Instance.GetAllPlantData())
            result.Add(new SelectedPlantData(data.plantId, data.plantType));
        return result;
    }
}
