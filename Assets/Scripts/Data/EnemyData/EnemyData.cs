using UnityEngine;

public class EnemyData
{
    [Header("Thông tin")]
    public int enemyId;
    public string enemyName;
    public Sprite icon;
    public PlantType enemyType;

    [Header("Prefab trong game")]
    public GameObject enemyPrefab;

    [Header("Chỉ số")]
    public float health;
    public float speed;
    public float armor;
    public float rechargeTime;
}
