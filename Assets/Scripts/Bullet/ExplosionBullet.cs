using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : BulletBase
{
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private LayerMask targetLayer = ~0;

    private static readonly Collider[] overlapBuffer = new Collider[64];
    private readonly HashSet<ZombieBase> damagedZombies = new HashSet<ZombieBase>();

    protected override bool TryHit(Collider other)
    {
        ZombieBase hitZombie = other.GetComponentInParent<ZombieBase>();
        if (hitZombie == null || !hitZombie.isAlive)
            return false;

        Explode(transform.position);
        return true;
    }

    private void Explode(Vector3 center)
    {
        damagedZombies.Clear();
        int count = Physics.OverlapSphereNonAlloc(center, explosionRadius, overlapBuffer, targetLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            ZombieBase zombie = overlapBuffer[i].GetComponentInParent<ZombieBase>();
            if (zombie == null || !zombie.isAlive || !damagedZombies.Add(zombie))
                continue;

            if (effectData != null)
                zombie.ApplyEffect(effectData);
            zombie.TakeDamage(damage);
        }

        damagedZombies.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
