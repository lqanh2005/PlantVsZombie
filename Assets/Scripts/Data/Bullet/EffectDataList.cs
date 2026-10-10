using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDataList", menuName = "Bullet/EffectDataList")]
public class EffectDataList : SingletonScriptableObject<EffectDataList>
{
    public List<EffectData> bulletDataList;
    public EffectData GetEffectDataByType(EffectType type)
    {
        return bulletDataList.Find(data => data.effectType == type);
    }
}
[System.Serializable]
public class EffectData
{
    [Header("Basic")]
    public int bulletId;
    public string bulletName;
    public GameObject bulletPrefab;

    [Header("Combat")]
    public float damage = 20f;
    public float speed = 8f;

    [Header("Effect")]
    public EffectType effectType;
    public float effectValue;
    public float effectDuration;
}
public enum EffectType
{
    Normal,
    Burn,
    ArmorBreak,
    Slow
}