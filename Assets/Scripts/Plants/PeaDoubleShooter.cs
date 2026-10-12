using DG.Tweening;
using UnityEngine;

public class PeaDoubleShooter : ShooterPlantBase
{
    override protected void Shoot()
    {
        SpawnBullet();
        DOVirtual.DelayedCall(0.2f, SpawnBullet).SetLink(gameObject);
    }
}
