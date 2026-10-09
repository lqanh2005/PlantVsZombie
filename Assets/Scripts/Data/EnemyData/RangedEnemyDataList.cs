using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RangedEnemyDataList", menuName = "Enemies/RangedEnemyDataList")]
public class RangedEnemyDataList : ScriptableObject
{
    public List<RangedEnemyData> rangedEnemyDataList = new List<RangedEnemyData>();
}

[System.Serializable]
public class RangedEnemyData : EnemyData
{
    public float damage;
    public float attackCooldown;
    public float attackRange;
    public GameObject bullet;

}
