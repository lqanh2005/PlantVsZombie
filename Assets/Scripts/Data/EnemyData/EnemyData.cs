using UnityEngine;

public abstract class EnemyData : ScriptableObject
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

public abstract class MeleEnemyData : EnemyData
{
    [Header("Cận chiến")]
    public float damage;
    public float attackCooldown;
}

public abstract class RangedEnemyData : EnemyData
{
    [Header("Đánh xa")]
    public float damage;
    public float attackCooldown;
    public float attackRange;
    public GameObject bullet;
}

public abstract class TankEnemyData : EnemyData
{
    [Header("Tank")]
    public float damage;
    public float attackCooldown;
    public float armorHealth;
    [Range(0f, 100f)] public float damageReduction;
}

public enum EnemyType
{
    Melee,
    Ranged,
    Tank,
    Boss
}
