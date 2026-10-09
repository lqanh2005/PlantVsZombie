using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefenderPlantData", menuName = "Plants/DefenderPlantData")]
public class DefenderPlantDataList : ScriptableObject
{
    public List<DefenderPlantData> defenderPlantDataList = new List<DefenderPlantData>();
}
[System.Serializable]
public class DefenderPlantData : PlantData
{
}
