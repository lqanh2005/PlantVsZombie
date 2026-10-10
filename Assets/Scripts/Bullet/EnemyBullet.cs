using UnityEngine;

public class EnemyBullet : BulletBase
{
    protected override bool TryHit(Collider other)
    {
        PlantBase plant = other.GetComponentInParent<PlantBase>();
        if (plant == null || !plant.isAlive)
            return false;

        plant.TakeDamage(effectData.damage);
        return true;
    }
}
