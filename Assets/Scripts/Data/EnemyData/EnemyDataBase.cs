using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Enemies/EnemyDataBase")]
public class EnemyDataBase : SingletonScriptableObject<EnemyDataBase>
{
    [SerializeField] List<EnemyData> enemyDatas = new List<EnemyData>();

    readonly Dictionary<EnemyType, List<EnemyData>> enemyDataDictionary = new Dictionary<EnemyType, List<EnemyData>>();

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void OnValidate()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        enemyDataDictionary.Clear();
        if (enemyDatas == null) return;

        foreach (EnemyData enemyData in enemyDatas)
        {
            if (enemyData == null) continue;

            if (!enemyDataDictionary.TryGetValue(enemyData.enemyType, out List<EnemyData> list))
            {
                list = new List<EnemyData>();
                enemyDataDictionary[enemyData.enemyType] = list;
            }
            list.Add(enemyData);
        }
    }

    public EnemyData GetEnemyData(EnemyType enemyType, int enemyId)
    {
        if (enemyDataDictionary.TryGetValue(enemyType, out List<EnemyData> list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].enemyId == enemyId)
                    return list[i];
            }
        }

        Debug.LogError($"Enemy data not found: type {enemyType}, id {enemyId}.");
        return null;
    }

    public T GetEnemyData<T>(EnemyType enemyType, int enemyId) where T : EnemyData
    {
        return GetEnemyData(enemyType, enemyId) as T;
    }

    public List<EnemyData> GetAllEnemyData()
    {
        List<EnemyData> result = new List<EnemyData>();
        if (enemyDatas == null) return result;

        for (int i = 0; i < enemyDatas.Count; i++)
        {
            if (enemyDatas[i] != null)
                result.Add(enemyDatas[i]);
        }
        return result;
    }
}
