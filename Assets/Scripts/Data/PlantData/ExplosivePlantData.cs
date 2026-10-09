using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ExplosivePlantData", menuName = "Plants/ExplosivePlantData")]
public class ExplosivePlantDataList : ScriptableObject
{
    public List<ExplosivePlantData> explosivePlantDataList  ;
}
public class ExplosivePlantData : PlantData
{
    public float explosionDamage;
    public float explosionRadius;
    private void OnValidate()
    {
        this.plantType = PlantType.Explosive;
    }
}
