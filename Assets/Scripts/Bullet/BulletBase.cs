using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    [SerializeField] protected EffectData effectData;
    [SerializeField] protected EffectType effectType;
    protected Vector3 direction;
    private bool hasHit;

    public virtual void Init(EffectType effectType)
    {
        effectData = EffectDataList.Instance.GetEffectDataByType(effectType);
        hasHit = false;
    }

    protected virtual void Update()
    {
        if (hasHit || effectData == null)
            return;

        transform.Translate(
            Vector3.right * effectData.speed * Time.deltaTime
        );
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        ZombieBase enemy = other.GetComponent<ZombieBase>();

        if (enemy == null)
            return;

        hasHit = true;

        enemy.GetEffect(effectData);
        enemy.TakeDamage(effectData.damage);
        Destroy(gameObject);
    }

    protected abstract bool TryHit(Collider other);
}
