using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ExplosivePlantData", menuName = "Plants/ExplosivePlantData")]
public class ExplosivePlantDataList : ScriptableObject
{
    public List<ExplosivePlantData> explosivePlantDataList = new List<ExplosivePlantData>();
}
[System.Serializable]
public class ExplosivePlantData : PlantData
{
    public float explosionDamage;
    public float explosionRadius;
}
