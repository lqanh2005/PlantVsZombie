using UnityEngine;

public abstract class MeleeEnemyBase : ZombieBase
{
    [SerializeField] protected float attackRange = 0.8f;

    protected MeleEnemyData MeleeData => (MeleEnemyData)data;
    protected override float AttackRange => attackRange;
    protected override float AttackCooldown => MeleeData.attackCooldown;

    protected override void Attack(PlantBase target)
    {
        target.TakeDamage(MeleeData.damage);
    }
}
