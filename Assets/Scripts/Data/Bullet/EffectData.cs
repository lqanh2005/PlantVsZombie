using UnityEngine;

public abstract class EffectData : ScriptableObject
{
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
    Slow,
    Explosion
}
