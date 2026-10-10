using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    protected EffectData effectData;
    protected float damage;
    protected float speed;
    protected Vector3 direction;
    private float lifeTimer;
    private bool hasHit;

    public virtual void Init(EffectType effectType)
    {
        effectData = EffectDataList.Instance.GetEffectDataByType(effectType);
        if (effectData == null)
        {
            SimplePool.Despawn(gameObject);
            return;
        }

        Setup(effectData.damage, Vector3.right, effectData.speed);
    }

    public virtual void Init(float damage, Vector3 direction, float speed)
    {
        effectData = null;
        Setup(damage, direction, speed);
    }

    private void Setup(float damage, Vector3 direction, float speed)
    {
        this.damage = damage;
        this.direction = direction.normalized;
        this.speed = speed;
        lifeTimer = lifeTime;
        hasHit = false;
    }

    protected virtual void Update()
    {
        if (hasHit)
            return;

        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            SimplePool.Despawn(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!TryHit(other))
            return;

        hasHit = true;
        SimplePool.Despawn(gameObject);
    }

    protected virtual bool TryHit(Collider other)
    {
        ZombieBase zombie = other.GetComponentInParent<ZombieBase>();
        if (zombie == null || !zombie.isAlive)
            return false;

        if (effectData != null)
            zombie.GetEffect(effectData);

        zombie.TakeDamage(damage);
        return true;
    }
}
