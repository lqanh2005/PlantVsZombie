using UnityEngine;

public class SlowBullet : BulletBase
{
    protected override bool TryHit(Collider other)
    {
        ZombieBase zombie = other.GetComponentInParent<ZombieBase>();
        if (zombie == null || !zombie.isAlive)
            return false;

        zombie.TakeDamage(effectData.damage);
        return true;
    }
}
