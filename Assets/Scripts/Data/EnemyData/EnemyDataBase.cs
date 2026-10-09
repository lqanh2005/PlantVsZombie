using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Enemies/EnemyDataBase")]
public class EnemyDataBase : SingletonScriptableObject<EnemyDataBase>
{
    public MeleEnemyDataList meleEnemyDataList;
    public RangedEnemyDataList rangedEnemyDataList;
    public TankEnemyDataList tankEnemyDataList;
    public EnemyData GetEnemyData(EnemyType enemyType, int idx)
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                return meleEnemyDataList.meleEnemyDataList[idx];
            case EnemyType.Ranged:
                return rangedEnemyDataList.rangedEnemyDataList[idx];
            case EnemyType.Tank:
                return tankEnemyDataList.tankEnemyDataList[idx];
            default:
                Debug.LogError($"Enemy type {enemyType} not found.");
                return null;
        }
    }
}
