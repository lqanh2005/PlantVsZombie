using UnityEngine;

public class PeaBullet : BulletBase
{
    protected override bool TryHit(Collider other)
    {
        ZombieBase zombie = other.GetComponentInParent<ZombieBase>();
        if (zombie == null || !zombie.isAlive)
            return false;

        zombie.TakeDamage(damage);
        return true;
    }
}
