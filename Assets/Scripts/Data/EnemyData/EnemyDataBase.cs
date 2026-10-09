using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Enemies/EnemyDataBase")]
public class EnemyDataBase : SingletonScriptableObject<EnemyDataBase>
{
    public MeleEnemyDataList meleEnemyDataList;
    public RangedEnemyDataList rangedEnemyDataList;
    public TankEnemyDataList tankEnemyDataList;
    public EnemyData GetEnemyData(EnemyType enemyType, int enemyId)
    {
        EnemyData result = null;
        switch (enemyType)
        {
            case EnemyType.Melee:
                result = meleEnemyDataList.meleEnemyDataList.Find(d => d.enemyId == enemyId);
                break;
            case EnemyType.Ranged:
                result = rangedEnemyDataList.rangedEnemyDataList.Find(d => d.enemyId == enemyId);
                break;
            case EnemyType.Tank:
                result = tankEnemyDataList.tankEnemyDataList.Find(d => d.enemyId == enemyId);
                break;
        }

        if (result == null)
            Debug.LogError($"Enemy data not found: type {enemyType}, id {enemyId}.");
        return result;
    }
}
