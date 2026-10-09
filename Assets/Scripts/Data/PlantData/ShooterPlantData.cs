using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShooterPlantData", menuName = "Plants/ShooterPlantData")]
public class ShooterPlantDataList : ScriptableObject
{
    public List<ShooterPlantData> shooterPlantDataList;
}
[System.Serializable]
public class ShooterPlantData : PlantData
{
    public float damage;
    public float attackCooldown;
    public float attackRange;
    private void OnValidate()
    {
        this.plantType = PlantType.Shooter;
    }
}
