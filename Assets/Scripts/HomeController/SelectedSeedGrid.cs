
using System.Collections.Generic;
using UnityEngine;

public class SelectedSeedGrid : MonoBehaviour
{
    private const string DefaultSlotContainerName = "SelectedSeedGrid";

    [SerializeField] private Transform slotContainer;
    [SerializeField] private SlotCardUI[] slots;
    [SerializeField] private PlantCardUI selectedCardPrefab;

    private class SelectedEntry
    {
        public PlantCardUI sourceCard;
        public PlantCardUI slotCard;
    }

    private readonly List<SelectedEntry> entries = new List<SelectedEntry>();

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
            slots = CollectSlots();
    }

    private SlotCardUI[] CollectSlots()
    {
        if (slotContainer == null)
            slotContainer = FindSlotContainer();

        if (slotContainer == null)
        {
            Debug.LogError("SelectedSeedGrid: Không tìm thấy container chứa slot!");
            return new SlotCardUI[0];
        }

        List<SlotCardUI> result = new List<SlotCardUI>();
        foreach (Transform child in slotContainer)
        {
            SlotCardUI slot = child.GetComponent<SlotCardUI>();
            if (slot == null)
                slot = child.gameObject.AddComponent<SlotCardUI>();
            result.Add(slot);
        }
        return result.ToArray();
    }

    private Transform FindSlotContainer()
    {
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Canvas canvas in canvases)
        {
            foreach (Transform t in canvas.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == DefaultSlotContainerName && t.childCount > 0)
                    return t;
            }
        }
        return null;
    }

    public void TrySelectPlant(PlantCardUI sourceCard)
    {
        if (sourceCard == null || sourceCard.IsSelected)
            return;

        if (entries.Count >= slots.Length)
            return;

        SlotCardUI targetSlot = slots[entries.Count];
        if (targetSlot == null)
            return;

        PlantData data = PlantDatabase.Instance.GetPlantData(
            sourceCard.plantType,
            sourceCard.plantId
        );
        if (data == null)
            return;

        PlantCardUI template = selectedCardPrefab != null ? selectedCardPrefab : sourceCard;

        // Spawn card mới tại vị trí card nguồn.
        PlantCardUI newCard = Instantiate(
            template,
            sourceCard.transform.position,
            Quaternion.identity,
            sourceCard.transform.parent
        );

        newCard.SetData(data);
        newCard.SetClickAction(DeselectPlant);

        sourceCard.SetSelected(true);
        entries.Add(new SelectedEntry { sourceCard = sourceCard, slotCard = newCard });
        RefreshSlots();

        // Di chuyển card mới đến slot đích.
        newCard.MoveToSlot(targetSlot.GetComponent<RectTransform>());
    }

    private void DeselectPlant(PlantCardUI slotCard)
    {
        int index = entries.FindIndex(e => e.slotCard == slotCard);
        if (index < 0)
            return;

        SelectedEntry entry = entries[index];
        entries.RemoveAt(index);

        if (entry.sourceCard != null)
            entry.sourceCard.SetSelected(false);
        Destroy(entry.slotCard.gameObject);

        for (int i = index; i < entries.Count; i++)
            entries[i].slotCard.SnapToSlot(slots[i].GetComponent<RectTransform>());

        RefreshSlots();
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].SetOccupied(i < entries.Count);
        }

        List<SelectedPlantData> selectedPlants = HomeController.Instance.selectedPlants;
        selectedPlants.Clear();
        foreach (SelectedEntry entry in entries)
            selectedPlants.Add(new SelectedPlantData(entry.sourceCard.plantId, entry.sourceCard.plantType));
    }
}
