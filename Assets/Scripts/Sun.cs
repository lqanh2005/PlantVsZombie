using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private int sunValue = 25;

    private bool isCollected;

    public void Collect()
    {
        if (isCollected)
            return;

        isCollected = true;

        Debug.Log($"Collected {sunValue} Sun");

        // TODO: Cộng sunValue vào tài nguyên người chơi.

        SimplePool.Despawn(gameObject);
    }
}