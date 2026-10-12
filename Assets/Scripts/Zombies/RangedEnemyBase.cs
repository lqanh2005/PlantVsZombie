using UnityEngine;

public abstract class RangedEnemyBase : ZombieBase
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float bulletSpeed = 6f;

    protected RangedEnemyData RangedData => (RangedEnemyData)data;
    protected override float AttackRange => RangedData.attackRange;
    protected override float AttackCooldown => RangedData.attackCooldown;

    protected override bool IsValidData(EnemyData data)
    {
        return data is RangedEnemyData;
    }

    protected override void Attack(PlantBase target)
    {
        GameObject prefab = bulletPrefab != null ? bulletPrefab : RangedData.bullet;
        if (prefab == null)
            return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        GameObject projectile = SimplePool.Spawn(prefab, spawnPosition, Quaternion.identity);

        BulletBase bullet = projectile.GetComponent<BulletBase>();
        if (bullet != null)
            bullet.Init(RangedData.damage, Vector3.left, bulletSpeed);
    }
}
