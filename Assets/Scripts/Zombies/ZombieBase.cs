using UnityEngine;

public abstract class ZombieBase : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected int enemyId;
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected float currentHealth;
    public bool isAlive { get; private set; }
    public virtual void Init()
    {
        isAlive = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
        SimplePool.Despawn(gameObject);
    }
}
