using System;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour, ITakeDamage
{
    protected int plantId;
    protected PlantType plantType;
    protected float currentHealth;
    public bool isAlive { get; private set; }
    protected virtual void Init() 
    {

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
