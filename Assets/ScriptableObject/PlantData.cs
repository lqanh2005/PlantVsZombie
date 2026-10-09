using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantData", order = 1)]
public class PlantData : ScriptableObject
{
    [Header("Thông tin")]
    public string plantName;

    [Header("Hình ảnh")]
    public Sprite icon;

    [Header("Prefab trong game")]
    public GameObject plantPrefab;

    [Header("Chỉ số")]
    public int sunCost;
    public float health;
    public float attackDamage;
    public float activeCooldown;
    public float rechargeTime;
}
