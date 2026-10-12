using UnityEngine;

public abstract class ShooterPlantBase : PlantBase
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    protected float attackTimer;
    protected bool hasFired;

    protected ShooterPlantData ShooterData => (ShooterPlantData)data;
    protected virtual float InitialAttackDelay => 2f;

    public override void Init(PlantData data)
    {
        base.Init(data);
        if (!isAlive)
            return;

        attackTimer = InitialAttackDelay;
        hasFired = false;
        OnCooldownChanged(false);
    }

    protected override bool IsValidData(PlantData data)
    {
        return data is ShooterPlantData;
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && CanAttack())
        {
            Shoot();
            hasFired = true;
            attackTimer = ShooterData.attackCooldown;
        }
        OnCooldownChanged(hasFired && attackTimer > 0f);
    }

    protected virtual bool CanAttack()
    {
        return FindZombieInFront(ShooterData.attackRange) != null;
    }

    protected virtual void Shoot()
    {
        SpawnBullet();
    }

    protected void SpawnBullet()
    {
        if (bulletPrefab == null || !isAlive)
            return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        GameObject projectile = SimplePool.Spawn(bulletPrefab, spawnPosition, Quaternion.identity);

        BulletBase bullet = projectile.GetComponent<BulletBase>();
        if (bullet != null)
            bullet.Init(ShooterData.damage, Vector3.right, ShooterData.bulletSpeed, GetBulletEffect());
    }

    private EffectData GetBulletEffect()
    {
        EffectType type = ShooterData.effectType;
        return type == EffectType.Normal ? null : EffectDataList.Instance.GetEffectDataByType(type);
    }
    protected virtual void OnCooldownChanged(bool isCoolingDown)
    {
        
    }
}
