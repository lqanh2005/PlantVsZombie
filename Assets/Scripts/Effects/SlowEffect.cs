using UnityEngine;

public class SlowEffect : StatusEffect
{
    public override EffectType EffectType => EffectType.Slow;

    public override float MoveSpeedMultiplier
    {
        get
        {
            return 1f - Mathf.Clamp01(effectData.effectValue);
        }
    }

    public override void Tick(ZombieBase target)
    {
        elapsedTime += Time.deltaTime;
    }

}
