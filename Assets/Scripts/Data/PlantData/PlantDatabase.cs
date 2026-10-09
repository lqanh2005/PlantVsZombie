using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlantDatabase", menuName = "Plants/PlantDatabase", order = 1)]
public class PlantDatabase : SingletonScriptableObject<PlantDatabase>
{
    public ShooterPlantDataList shooterPlantDataList;
    public ExplosivePlantDataList explosivePlantDataList;
    public SunProducerDataList sunProducerDataList;
    public DefenderPlantDataList defenderPlantDataList;

    public PlantData GetPlantDataByType(PlantType plantType, int index)
    {
        IReadOnlyList<PlantData> list = GetList(plantType);
        if (list == null || index < 0 || index >= list.Count)
        {
            Debug.LogError($"Plant data not found: type {plantType}, index {index}.");
            return null;
        }
        return list[index];
    }

    public PlantData GetPlantData(PlantType plantType, int plantId)
    {
        IReadOnlyList<PlantData> list = GetList(plantType);
        if (list != null)
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

    private IReadOnlyList<PlantData> GetList(PlantType plantType)
    {
        switch (plantType)
        {
            case PlantType.Shooter:
                return shooterPlantDataList != null ? shooterPlantDataList.shooterPlantDataList : null;
            case PlantType.Explosive:
                return explosivePlantDataList != null ? explosivePlantDataList.explosivePlantDataList : null;
            case PlantType.SunProducer:
                return sunProducerDataList != null ? sunProducerDataList.sunProducerDataList : null;
            case PlantType.Defense:
                return defenderPlantDataList != null ? defenderPlantDataList.defenderPlantDataList : null;
            default:
                return null;
        }
    }
}
