using UnityEngine;

public class ArmorBreakEffect : StatusEffect
{
    public override EffectType EffectType => EffectType.ArmorBreak;

    public override float ArmorMultiplier
    {
        get
        {
            return 1f - Mathf.Clamp01(effectData.effectValue);
        }
    }

    public override void Tick(ZombieBase target)
    {
        if (IsFinished)
            return;
        elapsedTime += Time.deltaTime;
    }
}
