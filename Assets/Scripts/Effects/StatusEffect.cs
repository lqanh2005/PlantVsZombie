using UnityEngine;

public abstract class StatusEffect
{
    public abstract EffectType EffectType { get; }
    protected EffectData effectData;

    protected float duration;
    protected float elapsedTime;
    protected float tickTimer;
    public virtual float MoveSpeedMultiplier => 1f;
    public virtual float ArmorMultiplier => 1f;
    public void Init(EffectData data)
    {
        effectData = data;
        duration = effectData.effectDuration;
        elapsedTime = 0f;
    }

    public virtual void OnApply(ZombieBase enemy)
    {
    }

    public abstract void Tick(ZombieBase enemy);

    public virtual void OnRemove(ZombieBase enemy)
    {
    }

    public bool IsFinished => elapsedTime >= duration;
}