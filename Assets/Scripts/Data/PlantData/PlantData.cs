using System;
using UnityEngine;

public class PlantData
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

public enum PlantType
{
    Shooter,
    Explosive,
    SunProducer,
    Buff,
    Defense
}