using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    protected float damage;
    protected float speed;
    protected Vector3 direction;
    private float lifeTimer;
    private bool hasHit;

    public virtual void Init(float damage, Vector3 direction, float speed)
    {
        this.damage = damage;
        this.direction = direction.normalized;
        this.speed = speed;
        lifeTimer = lifeTime;
        hasHit = false;
    }

    protected virtual void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            SimplePool.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!TryHit(other))
            return;

        hasHit = true;
        SimplePool.Despawn(gameObject);
    }

    protected abstract bool TryHit(Collider other);
}
