using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDataList", menuName = "Bullet/EffectDataList")]
public class EffectDataList : SingletonScriptableObject<EffectDataList>
{
    [SerializeField] List<EffectData> effectDatas = new List<EffectData>();

    readonly Dictionary<EffectType, EffectData> effectDataDictionary = new Dictionary<EffectType, EffectData>();

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
        effectDataDictionary.Clear();
        if (effectDatas == null) return;

        foreach (EffectData effectData in effectDatas)
        {
            if (effectData == null) continue;
            effectDataDictionary[effectData.effectType] = effectData;
        }
    }

    public EffectData GetEffectDataByType(EffectType type)
    {
        if (effectDataDictionary.TryGetValue(type, out EffectData effectData))
            return effectData;

        Debug.LogError($"Effect data not found: type {type}.");
        return null;
    }

    public List<EffectData> GetAllEffectData()
    {
        List<EffectData> result = new List<EffectData>();
        if (effectDatas == null) return result;

        for (int i = 0; i < effectDatas.Count; i++)
        {
            if (effectDatas[i] != null)
                result.Add(effectDatas[i]);
        }
        return result;
    }
}
