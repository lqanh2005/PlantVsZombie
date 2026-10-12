using UnityEngine;

public abstract class PlantBase : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected LayerMask zombieLayer;
    [SerializeField] protected float currentHealth;

    [Header("Phát hiện zombie")]
    [SerializeField] protected float laneHalfWidth = 0.4f;
    [SerializeField] protected float detectHeight = 2f;

    protected PlantData data;
    protected DamageFlash damageFlash;
    public bool isAlive { get; private set; }
    public bool IsAlive => isAlive;
    public int Row { get; private set; } = -1;
    public int Col { get; private set; } = -1;

    protected virtual void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        if (damageFlash == null)
            damageFlash = gameObject.AddComponent<DamageFlash>();
    }

    public virtual void Init(PlantData data)
    {
        if (!IsValidData(data))
        {
            Debug.LogError($"{name}: PlantData không hợp lệ ({(data != null ? data.GetType().Name : "null")}).");
            isAlive = false;
            SimplePool.Despawn(gameObject);
            return;
        }

        this.data = data;
        currentHealth = data.health;
        isAlive = true;
    }

    protected virtual bool IsValidData(PlantData data)
    {
        return data != null;
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
        Vector3 origin = transform.position + Vector3.up * (detectHeight * 0.5f);
        Vector3 direction = Vector3.right;
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, range, zombieLayer, QueryTriggerInteraction.Collide);

        ZombieBase nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance >= nearestDistance)
                continue;

            ZombieBase zombie = hit.collider.GetComponentInParent<ZombieBase>();
            if (zombie == null || !zombie.isAlive)
                continue;

            nearest = zombie;
            nearestDistance = hit.distance;
        }

        return nearest;
    }
}
