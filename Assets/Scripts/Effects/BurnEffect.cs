using UnityEngine;

public class BurnEffect : StatusEffect
{
    public override EffectType EffectType => EffectType.Burn;

    public override void OnApply(ZombieBase target)
    {
        base.OnApply(target);
        tickTimer = 0f;
    }

    public override void Tick(ZombieBase target)
    {
        if (IsFinished)
            return;

        float deltaTime = Mathf.Min(
            Time.deltaTime,
            effectData.effectDuration - elapsedTime
        );

        elapsedTime += deltaTime;
        tickTimer += deltaTime;

        // Gây sát thương mỗi 1 giây
        while (tickTimer >= 1f)
        {
            tickTimer -= 1f;

            target.TakeDamage(effectData.effectValue);
        }
    }
}