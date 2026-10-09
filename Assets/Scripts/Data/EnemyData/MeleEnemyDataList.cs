using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "MeleEnemyDataList", menuName = "Enemies/MeleEnemyDataList")]
public class MeleEnemyDataList : ScriptableObject
{
    public List<MeleEnemyData> meleEnemyDataList = new List<MeleEnemyData>();
}
[System.Serializable]
public class MeleEnemyData : EnemyData
{
    public float damage;
    public float attackCooldown;
    
}
