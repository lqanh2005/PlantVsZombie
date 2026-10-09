using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TankEnemyDataList", menuName = "Enemies/TankEnemyDataList")]
public class TankEnemyDataList : ScriptableObject
{
    public List<TankEnemyData> tankEnemyDataList = new List<TankEnemyData>();
}

[System.Serializable]
public class TankEnemyData : EnemyData
{
    public float armorHealth;
    public float damageReduction;
}
