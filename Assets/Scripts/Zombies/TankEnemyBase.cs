using UnityEngine;

public abstract class TankEnemyBase : ZombieBase
{
    [SerializeField] protected float attackRange = 0.8f;
    [SerializeField] protected GameObject armorVisual;

    protected float currentArmorHealth;

    protected TankEnemyData TankData => (TankEnemyData)data;
    protected override float AttackRange => attackRange;
    protected override float AttackCooldown => TankData.attackCooldown;
    public bool HasArmor => currentArmorHealth > 0f;

    public override void Init()
    {
        base.Init();
        if (!isAlive)
            return;

        currentArmorHealth = TankData.armorHealth;
        if (armorVisual != null)
            armorVisual.SetActive(true);
    }

    protected override void Attack(PlantBase target)
    {
        target.TakeDamage(TankData.damage);
    }

    public override void TakeDamage(float damage)
    {
        if (!isAlive)
            return;

        if (HasArmor)
        {
            float reduced = damage * (1f - Mathf.Clamp(TankData.damageReduction, 0f, 100f) / 100f);
            currentArmorHealth -= reduced;
            if (currentArmorHealth > 0f)
            {
                PlayHitFlash();
                return;
            }

            damage = -currentArmorHealth;
            currentArmorHealth = 0f;
            OnArmorBroken();
            if (damage <= 0f)
            {
                PlayHitFlash();
                return;
            }
        }

        base.TakeDamage(damage);
    }

    protected virtual void OnArmorBroken()
    {
        if (armorVisual != null)
            armorVisual.SetActive(false);
    }
}
