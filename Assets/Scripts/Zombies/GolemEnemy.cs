using UnityEngine;

public class GolemEnemy : TankEnemyBase
{
    [SerializeField] private float enragedSpeedMultiplier = 1.5f;

    protected override float MoveSpeed => HasArmor ? data.speed : data.speed * enragedSpeedMultiplier;
}
