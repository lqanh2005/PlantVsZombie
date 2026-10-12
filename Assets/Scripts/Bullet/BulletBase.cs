using DG.Tweening;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    [Header("VFX")]
    [Tooltip("Prefab VFX riêng (không phải object con của đạn)")]
    [SerializeField] protected GameObject despawnVfx;
    [Tooltip("Thời gian tồn tại của VFX, <= 0 = tự tính theo ParticleSystem")]
    [SerializeField] protected float vfxLifetime = 0f;
    [SerializeField] protected bool playVfxOnExpire = true;
    [SerializeField] protected Rigidbody rb;

    protected EffectData effectData;
    protected float damage;
    protected float speed;
    protected Vector3 direction;
    private float lifeTimer;
    private bool hasHit;

    protected virtual void Awake()
    {
        if (despawnVfx != null && despawnVfx.transform.IsChildOf(transform))
        {
            Debug.LogWarning($"{name}: despawnVfx phải là prefab riêng, không phải object con của đạn.");
            despawnVfx = null;
        }
    }

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

    private void Setup(float damage, Vector3 direction, float speed)
    {
        this.damage = damage;
        this.direction = direction.normalized;
        this.speed = speed;
        lifeTimer = lifeTime;
        hasHit = false;
    }

    protected virtual void FixedUpdate()
    {
        if (hasHit || rb == null)
            return;

        rb.MovePosition(
            rb.position + direction.normalized * speed * Time.fixedDeltaTime
        );
    }

    protected virtual void Update()
    {
        if (hasHit)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
            Despawn(playVfxOnExpire);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!TryHit(other))
            return;

        hasHit = true;
        Despawn(true);
    }

    protected virtual bool TryHit(Collider other)
    {
        ITakeDamage target = other.GetComponentInParent<ITakeDamage>();

        if (target == null)
            return false;

        if (effectData != null && target is IStatusEffectReceiver effectReceiver)
        {
            effectReceiver.ApplyEffect(effectData);
        }
        target.TakeDamage(damage);
        return true;
    }

    protected void Despawn(bool playVfx)
    {
        if (playVfx)
            PlayDespawnVfx();

        SimplePool.Despawn(gameObject);
    }

    private void PlayDespawnVfx()
    {
        if (despawnVfx == null)
            return;

        GameObject vfxObject = SimplePool.Spawn(despawnVfx, transform.position, Quaternion.identity);
        ParticleSystem[] systems = vfxObject.GetComponentsInChildren<ParticleSystem>(true);

        float duration = 0f;
        foreach (ParticleSystem ps in systems)
        {
            ps.Clear(false);
            ps.Play(false);

            ParticleSystem.MainModule main = ps.main;
            duration = Mathf.Max(duration, main.startDelay.constantMax + main.duration + main.startLifetime.constantMax);
        }

        if (vfxLifetime > 0f)
            duration = vfxLifetime;

        DOVirtual.DelayedCall(Mathf.Max(0.1f, duration), () => SimplePool.Despawn(vfxObject)).SetLink(vfxObject);
    }
}
