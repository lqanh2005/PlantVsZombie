using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDatabase", menuName = "Plants/PlantDatabase", order = 1)]
public class PlantDatabase : SingletonScriptableObject<PlantDatabase>
{
    [SerializeField] List<PlantData> plantDatas = new List<PlantData>();

    readonly Dictionary<PlantType, List<PlantData>> plantDataDictionary = new Dictionary<PlantType, List<PlantData>>();

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void OnValidate()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        plantDataDictionary.Clear();
        if (plantDatas == null) return;

        foreach (PlantData plantData in plantDatas)
        {
            if (plantData == null) continue;

            if (!plantDataDictionary.TryGetValue(plantData.plantType, out List<PlantData> list))
            {
                list = new List<PlantData>();
                plantDataDictionary[plantData.plantType] = list;
            }
            list.Add(plantData);
        }
    }

    public PlantData GetPlantDataByType(PlantType plantType, int index)
    {
        if (!plantDataDictionary.TryGetValue(plantType, out List<PlantData> list) || index < 0 || index >= list.Count)
        {
            Debug.LogError($"Plant data not found: type {plantType}, index {index}.");
            return null;
        }
        return list[index];
    }

    public PlantData GetPlantData(PlantType plantType, int plantId)
    {
        if (plantDataDictionary.TryGetValue(plantType, out List<PlantData> list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].plantId == plantId)
                    return list[i];
            }
        }

        Debug.LogError($"Plant data not found: type {plantType}, id {plantId}.");
        return null;
    }

    public T GetPlantData<T>(PlantType plantType, int plantId) where T : PlantData
    {
        return GetPlantData(plantType, plantId) as T;
    }

    public List<PlantData> GetAllPlantData()
    {
        List<PlantData> result = new List<PlantData>();
        if (plantDatas == null) return result;

        for (int i = 0; i < plantDatas.Count; i++)
        {
            if (plantDatas[i] != null)
                result.Add(plantDatas[i]);
        }
        return result;
    }
}
