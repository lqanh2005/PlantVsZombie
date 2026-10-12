using DG.Tweening;
using UnityEngine;

public class PeaDoubleShooter : ShooterPlantBase
{
    override protected void Shoot()
    {
        if (bulletPrefab == null)
            return;
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        // Spawn the first bullet
        GameObject projectile1 = SimplePool.Spawn(bulletPrefab, spawnPosition, Quaternion.identity);
        BulletBase bullet1 = projectile1.GetComponent<BulletBase>();
        if (bullet1 != null)
            bullet1.Init(this.effectType);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            // Spawn the second bullet with a slight offset
            Vector3 spawnPosition2 = firePoint != null ? firePoint.position : transform.position + Vector3.up;
            GameObject projectile2 = SimplePool.Spawn(bulletPrefab, spawnPosition2, Quaternion.identity);
            BulletBase bullet2 = projectile2.GetComponent<BulletBase>();
            if (bullet2 != null)
                bullet2.Init(this.effectType);
        });
    }
}
