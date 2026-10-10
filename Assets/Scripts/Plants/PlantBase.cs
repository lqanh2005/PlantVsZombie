using UnityEngine;

public abstract class PlantBase : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int plantId;
    [SerializeField] protected PlantType plantType;
    [SerializeField] protected EffectType effectType;

    [SerializeField] protected float currentHealth;

    [Header("Phát hiện zombie")]
    [SerializeField] protected float laneHalfWidth = 0.4f;
    [SerializeField] protected float detectHeight = 2f;

    protected PlantData data;
    protected DamageFlash damageFlash;
    public bool isAlive { get; private set; }
    public int Row { get; private set; } = -1;
    public int Col { get; private set; } = -1;

    protected virtual void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        if (damageFlash == null)
            damageFlash = gameObject.AddComponent<DamageFlash>();
    }

    public virtual void Init()
    {
        data = PlantDatabase.Instance.GetPlantData(plantType, plantId);
        if (data == null)
        {
            isAlive = false;
            SimplePool.Despawn(gameObject);
            return;
        }

        currentHealth = data.health;
        isAlive = true;
    }

    public void SetCell(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public virtual void TakeDamage(float damage)
    {
        if (!isAlive)
            return;

        currentHealth -= damage * 100f / (100f + Mathf.Max(0f, data.armor));
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (damageFlash != null)
            damageFlash.Play();
    }

    protected virtual void Die()
    {
        isAlive = false;

        PlantSpawner spawner = GamePlayController.Instance.playerContain.plantSpawner;
        if (spawner != null)
            spawner.FreeCell(Row, Col);

        SimplePool.Despawn(gameObject);
    }

    protected ZombieBase FindZombieInFront(float range)
    {
        Vector3 center = transform.position + Vector3.right * range * 0.5f + Vector3.up * detectHeight * 0.5f;
        Vector3 halfExtents = new Vector3(range * 0.5f, detectHeight * 0.5f, laneHalfWidth);
        Collider[] hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity, ~0, QueryTriggerInteraction.Collide);

        ZombieBase nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (Collider hit in hits)
        {
            ZombieBase zombie = hit.GetComponentInParent<ZombieBase>();
            if (zombie == null || !zombie.isAlive)
                continue;

            float distance = zombie.transform.position.x - transform.position.x;
            if (distance < 0f || distance >= nearestDistance)
                continue;

            nearest = zombie;
            nearestDistance = distance;
        }
        return nearest;
    }
}
