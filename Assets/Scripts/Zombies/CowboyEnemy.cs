using UnityEngine;

public class CowboyEnemy : MeleeEnemyBase
{
    [SerializeField] private float dashMultiplier = 2f;
    private bool hasDashed;

    public override void Init()
    {
        base.Init();
        hasDashed = false;
    }

    protected override float MoveSpeed => hasDashed ? data.speed : data.speed * dashMultiplier;

    protected override void Attack(PlantBase target)
    {
        hasDashed = true;
        base.Attack(target);
    }
}
