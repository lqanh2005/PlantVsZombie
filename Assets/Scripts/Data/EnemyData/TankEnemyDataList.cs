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
    public float damage;
    public float attackCooldown;
    public float armorHealth;
    [Range(0f, 100f)] public float damageReduction;
}
