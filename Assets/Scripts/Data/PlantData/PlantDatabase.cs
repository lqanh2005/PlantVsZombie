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
        switch (plantType)
        {
            case PlantType.Shooter:
                return shooterPlantDataList.shooterPlantDataList[index];
            case PlantType.Explosive:
                return explosivePlantDataList.explosivePlantDataList[index];
            case PlantType.SunProducer:
                return sunProducerDataList.sunProducerDataList[index];
            case PlantType.Defense:
                return defenderPlantDataList.defenderPlantDataList[index];
            default:
                return null;
        }
    }
}
