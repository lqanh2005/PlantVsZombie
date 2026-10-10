using System;
using System.Collections.Generic;

[Serializable]
public class SelectedPlantList
{
    public List<SelectedPlantData> plants = new List<SelectedPlantData>();
}
[Serializable]
public class SelectedPlantData
{
    public int plantId;
    public PlantType plantType;

    public SelectedPlantData(int id, PlantType type)
    {
        plantId = id;
        plantType = type;
    }

}
