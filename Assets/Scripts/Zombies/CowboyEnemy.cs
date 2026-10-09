using UnityEngine;

public class CowboyEnemy : MeleeEnemyBase
{
    [SerializeField] private MeleEnemyData meleEnemyData;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float attackTimer;

    public override void Init()
    {
        base.Init();
        meleEnemyData = (MeleEnemyData)EnemyDataBase.Instance.GetEnemyData(enemyType, enemyId - 1);
        currentHealth = meleEnemyData.health;
        attackTimer = meleEnemyData.attackCooldown;
    }
    protected virtual void Update()
    {
        if (!isAlive)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && CanAttack())
        {
            attackTimer = meleEnemyData.attackCooldown;
        }
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
