using UnityEngine;

public class BucketheadEnemy : TankEnemyBase
{
    [SerializeField] private TankEnemyData tankEnemyData;
    private float attackTimer;

    public override void Init()
    {
        base.Init();
        tankEnemyData = (TankEnemyData)EnemyDataBase.Instance.GetEnemyData(enemyType, enemyId - 1);
        currentHealth = tankEnemyData.health;
    }
    protected virtual void Update()
    {
        if (!isAlive)
            return;

    }
    protected virtual bool CanAttack()
    {
        // TODO: Kiểm tra có Zombie trong tầm bắn hay không.
        return true;
    }

    //public override void ResetPlant()
    //{
    //    base.ResetPlant();
    //    attackTimer = 0f;
    //}
}
