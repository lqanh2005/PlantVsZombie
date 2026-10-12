using UnityEngine;

public abstract class PlantData : ScriptableObject
{
    [Header("Thông tin")]
    public int plantId;
    public string plantName;
    public Sprite icon;
    public PlantType plantType;

    [Header("Prefab trong game")]
    public GameObject plantPrefab;

    [Header("Chỉ số")]
    public int sunCost;
    public float health;
    public float armor;
    public float rechargeTime;
}

public abstract class ShooterPlantData : PlantData
{
    [Header("Tấn công")]
    public EffectType effectType;
    public float damage;
    public float bulletSpeed = 15f;
    public float attackCooldown;
    public float attackRange;
}

public abstract class SunProducerData : PlantData
{
    [Header("Sản xuất mặt trời")]
    public int sunAmount;
    public float productionInterval;
}

public abstract class DefenderPlantData : PlantData
{
}

public abstract class ExplosivePlantData : PlantData
{
    [Header("Nổ")]
    public float explosionDamage;
    public float explosionRadius;
}

public enum PlantType
{
    Shooter,
    Explosive,
    SunProducer,
    Buff,
    Defense
}
