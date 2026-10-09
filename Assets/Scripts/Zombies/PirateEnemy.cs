using System.Collections;
using UnityEngine;

public class PirateEnemy : RangedEnemyBase
{
    [SerializeField] private int burstCount = 2;
    [SerializeField] private float burstInterval = 0.2f;

    protected override void Attack(PlantBase target)
    {
        StartCoroutine(BurstFire(target));
    }

    private IEnumerator BurstFire(PlantBase target)
    {
        for (int i = 0; i < burstCount; i++)
        {
            if (!isAlive)
                yield break;

            base.Attack(target);
            yield return new WaitForSeconds(burstInterval);
        }
    }
}
