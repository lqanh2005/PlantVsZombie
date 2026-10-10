
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private readonly List<StatusEffect> activeEffects = new();
    [SerializeField] private ZombieBase zombie;

    private void OnValidate()
    {
        zombie = GetComponent<ZombieBase>();
    }

    public void ApplyEffect(EffectData effectData)
    {
        if (effectData == null || zombie == null || !zombie.isAlive)
            return;
        StatusEffect effect = CreateEffect(effectData.effectType);

        if (effect == null)
            return;

        effect.Init(effectData);

        // Chính sách hiện tại: effect cùng loại thay thế effect cũ.
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i].EffectType != effectData.effectType)
                continue;

            activeEffects[i].OnRemove(zombie);
            activeEffects.RemoveAt(i);
        }

        activeEffects.Add(effect);
        effect.OnApply(zombie);
    }

    private void Update()
    {
        if (zombie == null || !zombie.isAlive)
            return;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect effect = activeEffects[i];
            effect.Tick(zombie);

            if (!effect.IsFinished)
                continue;

            effect.OnRemove(zombie);
            activeEffects.RemoveAt(i);
        }
    }

    public float GetMoveSpeedMultiplier()
    {
        float multiplier = 1f;

        foreach (StatusEffect effect in activeEffects)
            multiplier *= effect.MoveSpeedMultiplier;
        return multiplier;
    }

    public float GetArmorMultiplier()
    {
        float multiplier = 1f;

        foreach (StatusEffect effect in activeEffects)
            multiplier *= effect.ArmorMultiplier;

        return multiplier;
    }

    private StatusEffect CreateEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.Burn:
                return new BurnEffect();

            case EffectType.Slow:
                return new SlowEffect();
            case EffectType.ArmorBreak:
                return new ArmorBreakEffect();
            default:
                Debug.LogWarning($"Unsupported effect type: {type}");
                return null;
        }
    }

    public void ClearAllEffects()
    {
        foreach (StatusEffect effect in activeEffects)
            effect.OnRemove(zombie);

        activeEffects.Clear();
    }

}
