using UnityEngine;

public interface ITakeDamage
{
    bool IsAlive { get; }
    void TakeDamage(float damage);
}
