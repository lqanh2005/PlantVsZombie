using UnityEngine;

public abstract class ShooterPlantBase : PlantBase
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
     protected float attackTimer;

    protected ShooterPlantData ShooterData => (ShooterPlantData)data;

    public override void Init()
    {
        base.Init();
        if (!isAlive)
            return;

        attackTimer = 2f;
        OnCooldownChanged(false);
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && CanAttack())
        {
            Shoot();
            attackTimer = ShooterData.attackCooldown;
        }
        OnCooldownChanged(attackTimer > 0f);
    }

    protected virtual bool CanAttack()
    {
        return FindZombieInFront(ShooterData.attackRange) != null;
    }

    protected virtual void Shoot()
    {
        if (bulletPrefab == null)
            return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        GameObject projectile = SimplePool.Spawn(bulletPrefab, spawnPosition, Quaternion.identity);

        BulletBase bullet = projectile.GetComponent<BulletBase>();
        if (bullet != null)
            bullet.Init(this.effectType);
    }
    protected virtual void OnCooldownChanged(bool isCoolingDown)
    {
        
    }
}
