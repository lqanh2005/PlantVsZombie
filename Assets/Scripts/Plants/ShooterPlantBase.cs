using UnityEngine;

public class ShooterPlantBase : PlantBase
{
    [SerializeField] private ShooterPlantData shooterPlantData;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float attackTimer;

    public override void Init()
    {
        base.Init();
        shooterPlantData = (ShooterPlantData)PlantDatabase.Instance.GetPlantDataByType(plantType, plantId-1);
        currentHealth = shooterPlantData.health;
        attackTimer = shooterPlantData.attackCooldown;
    }
    protected virtual void Update()
    {
        if (!isAlive)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && CanAttack())
        {
            Shoot();
            attackTimer = shooterPlantData.attackCooldown;
        }
    }
    protected virtual bool CanAttack()
    {
        // TODO: Kiểm tra có Zombie trong tầm bắn hay không.
        return true;
    }

    protected virtual void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject projectile = SimplePool.Spawn(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        // TODO: Truyền damage cho projectile nếu cần.
    }
    //public override void ResetPlant()
    //{
    //    base.ResetPlant();
    //    attackTimer = 0f;
    //}
}
