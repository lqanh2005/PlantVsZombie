using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(EffectController))]
public abstract class ZombieBase : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int enemyId;
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected EnemyAnimation animator;
    [SerializeField] protected EffectController effectController;

    [Header("Phát hiện cây")]
    [SerializeField] protected float laneHalfWidth = 0.4f;
    [SerializeField] protected float detectHeight = 2f;

    [Header("Nhà")]
    [SerializeField] protected float houseOffset = 1f;

    [Header("Hướng model")]
    [SerializeField] protected float modelYawOffset = 0f;

    protected EnemyData data;
    protected float attackTimer;
    protected float houseX;
    protected DamageFlash damageFlash;
    public bool isAlive { get; private set; }

    protected abstract float AttackRange { get; }
    protected abstract float AttackCooldown { get; }
    protected abstract void Attack(PlantBase target);

    private void OnValidate()
    {
        animator = GetComponent<EnemyAnimation>();
        damageFlash = GetComponent<DamageFlash>();
        effectController = GetComponent<EffectController>();
    }

    protected void PlayHitFlash()
    {
        if (damageFlash != null)
            damageFlash.Play();
    }

    public virtual void Init()
    {
        data = EnemyDataBase.Instance.GetEnemyData(enemyType, enemyId);
        if (data == null)
        {
            isAlive = false;
            SimplePool.Despawn(gameObject);
            return;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.left) * Quaternion.Euler(0f, modelYawOffset, 0f);
        currentHealth = data.health;
        attackTimer = 0f;
        houseX = GetHouseX();
        isAlive = true;
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        if (GamePlayController.Instance.stateGame == StateGame.Lose)
            return;

        attackTimer -= Time.deltaTime;

        PlantBase target = FindPlantInFront(AttackRange);
        if (target == null)
        {
            Move();
            return;
        }

        if (attackTimer <= 0f)
        {
            Attack(target);
            attackTimer = AttackCooldown;
        }
    }

    protected virtual float MoveSpeed
    {
        get
        {
            float multiplier = effectController != null
                ? effectController.GetMoveSpeedMultiplier()
                : 1f;
            return data.speed * multiplier;
        }
    }
    protected virtual float Armor
    {
        get
        {
            float multiplier = effectController != null
                ? effectController.GetArmorMultiplier()
                : 1f;
            return data.armor * multiplier;
        }
    }

    protected virtual void Move()
    {
        float currentSpeed = MoveSpeed;

        transform.Translate(
            Vector3.left * currentSpeed * Time.deltaTime,
            Space.World
        );

        if (transform.position.x <= houseX)
            OnReachHouse();

        animator.SetMoving(true);
    }

    protected virtual void OnReachHouse()
    {
        GamePlayController.Instance.stateGame = StateGame.Lose;
        isAlive = false;
        SimplePool.Despawn(gameObject);
    }

    protected PlantBase FindPlantInFront(float range)
    {
        Vector3 center = transform.position + Vector3.left * range * 0.5f + Vector3.up * detectHeight * 0.5f;
        Vector3 halfExtents = new Vector3(range * 0.5f, detectHeight * 0.5f, laneHalfWidth);
        Collider[] hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, ~0, QueryTriggerInteraction.Collide);

        PlantBase nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (Collider hit in hits)
        {
            PlantBase plant = hit.GetComponentInParent<PlantBase>();
            if (plant == null || !plant.isAlive)
                continue;

            float distance = transform.position.x - plant.transform.position.x;
            if (distance < -laneHalfWidth || distance >= nearestDistance)
                continue;

            nearest = plant;
            nearestDistance = distance;
        }
        return nearest;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive)
            return;

        currentHealth -= ApplyArmor(damage);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        animator.TakeDamage();
        PlayHitFlash();
    }

    protected float ApplyArmor(float damage)
    {
        return damage * 100f / (100f + Mathf.Max(0f, Armor));
    }

    protected virtual void Die()
    {
        isAlive = false;
        animator.Die();
        DOVirtual.DelayedCall(1f, () => SimplePool.Despawn(gameObject));
        SimplePool.Despawn(gameObject);
    }

    private float GetHouseX()
    {
        GridController grid = GamePlayController.Instance.playerContain.grid;
        return grid.transform.TransformPoint(new Vector3(-grid.boardWidth / 2f, 0f, 0f)).x - houseOffset;
    }
    public void GetEffect(EffectData effect)
    {
        effectController.ApplyEffect(effect);
    }

}
