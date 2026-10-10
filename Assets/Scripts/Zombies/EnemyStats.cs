using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseArmor = 10f;
    [SerializeField] private float baseMoveSpeed = 2f;
    [SerializeField] private float baseAttackSpeed = 1f;

    private readonly List<StatModifier> modifiers = new();

    public float Armor => GetValue(StatType.Armor);
    public float MoveSpeed => GetValue(StatType.MoveSpeed);
    public float AttackSpeed => GetValue(StatType.AttackSpeed);

    public void AddModifier(StatModifier modifier)
    {
        if (modifier == null)
            return;

        modifiers.Add(modifier);
    }

    public void RemoveModifiers(object source)
    {
        modifiers.RemoveAll(m => m.Source == source);
    }

    public float GetValue(StatType stat)
    {
        float baseValue = stat switch
        {
            StatType.Armor => baseArmor,
            StatType.MoveSpeed => baseMoveSpeed,
            StatType.AttackSpeed => baseAttackSpeed,
            _ => 0f
        };

        float additive = 0f;
        float multiplier = 1f;

        foreach (var modifier in modifiers)
        {
            if (modifier.Stat != stat)
                continue;

            if (modifier.Type == ModifierType.Additive)
                additive += modifier.Value;
            else
                multiplier *= modifier.Value;
        }

        return Mathf.Max(0f, (baseValue + additive) * multiplier);
    }
}
public enum StatType
{
    Armor,
    MoveSpeed,
    AttackSpeed
}

public enum ModifierType
{
    Additive,
    Multiplicative
}
public sealed class StatModifier
{
    public object Source { get; }
    public StatType Stat { get; }
    public ModifierType Type { get; }
    public float Value { get; }

    public StatModifier(
        object source,
        StatType stat,
        ModifierType type,
        float value)
    {
        Source = source;
        Stat = stat;
        Type = type;
        Value = value;
    }
}