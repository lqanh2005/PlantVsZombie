using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SunProducerData", menuName = "Plants/SunProducerData")]    
public class SunProducerDataList : ScriptableObject
{
    public List<SunProducerData> sunProducerDataList = new List<SunProducerData>();
}
[System.Serializable]
public class SunProducerData : PlantData
{
    public int sunAmount;
    public float productionInterval;
    private void OnValidate()
    {
        this.plantType = PlantType.SunProducer;
    }
}
