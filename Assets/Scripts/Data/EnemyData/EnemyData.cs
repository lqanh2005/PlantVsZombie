using UnityEngine;

public class EnemyData
{
    [Header("Thông tin")]
    public int enemyId;
    public string enemyName;
    public EnemyType enemyType;

    [Header("Prefab trong game")]
    public GameObject enemyPrefab;

    [Header("Chỉ số")]
    public float health;
    public float speed;
    public float armor;
}

public enum EnemyType
{
    Melee,
    Ranged,
    Tank,
    Boss
}