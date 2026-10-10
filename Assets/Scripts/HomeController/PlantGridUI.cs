using System.Collections.Generic;
using UnityEngine;

public class PlantGridUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private PlantCardUI plantCardPrefab;

    private readonly List<PlantCardUI> plantCards = new();

    public void Init()
    {
        ClearCards();

        List<PlantData> plantDatas =
            PlantDatabase.Instance.GetAllPlantData();

        Transform viewport = content.Find("Viewport");

        if (viewport == null)
        {
            Debug.LogError("Không tìm thấy Viewport!");
            return;
        }

        int slotCount = viewport.childCount;

        for (int i = 0; i < Mathf.Min(slotCount, plantDatas.Count); i++)
        {
            Transform slot = viewport.GetChild(i);

            PlantCardUI card = Instantiate(plantCardPrefab, slot, false);

            RectTransform rect = card.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            card.SetData(plantDatas[i]);
            plantCards.Add(card);
        }
    }

    private void ClearCards()
    {
        foreach (PlantCardUI card in plantCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }

        plantCards.Clear();
    }
}