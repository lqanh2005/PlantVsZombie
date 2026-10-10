using UnityEngine;

public class SkySunSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sunPrefab;
    [SerializeField] private int sunAmount = 25;

    [Header("Thời gian spawn")]
    [SerializeField] private float firstDelay = 6f;
    [SerializeField] private float minInterval = 8f;
    [SerializeField] private float maxInterval = 12f;

    [Header("Rơi")]
    [SerializeField] private float fallSpeed = 1.5f;
    [SerializeField] private float groundHeight = 0.5f;
    [SerializeField] private float groundLifetime = 10f;
    [Tooltip("Vị trí xuất phát theo viewport, > 1 là phía trên mép màn hình")]
    [SerializeField] private float spawnViewportY = 1.15f;

    private float timer;
    private bool isRunning;

    public void Init()
    {
        timer = firstDelay;
        isRunning = sunPrefab != null;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        StateGame state = GamePlayController.Instance.stateGame;
        if (state == StateGame.Lose || state == StateGame.Win)
            return;

        timer -= Time.deltaTime;
        if (timer > 0f)
            return;

        SpawnSun();
        timer = Random.Range(minInterval, maxInterval);
    }

    private void SpawnSun()
    {
        GridController grid = GamePlayController.Instance.playerContain.grid;
        int row = Random.Range(0, grid.Rows);
        int col = Random.Range(0, grid.Cols);
        Vector3 target = grid.GetCellCenterWorld(row, col) + Vector3.up * groundHeight;
        Vector3 start = GetSpawnPosition(target);

        GameObject sunObject = SimplePool.Spawn(sunPrefab, start, Quaternion.identity);
        Sun sun = sunObject.GetComponent<Sun>();
        if (sun == null)
            return;

        sun.Init(sunAmount);
        float duration = Vector3.Distance(start, target) / Mathf.Max(0.1f, fallSpeed);
        sun.FallTo(target, duration, groundLifetime);
    }

    private Vector3 GetSpawnPosition(Vector3 target)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return target + Vector3.up * 10f;

        Vector3 viewport = cam.WorldToViewportPoint(target);
        Ray ray = cam.ViewportPointToRay(new Vector3(viewport.x, spawnViewportY, 0f));
        Plane plane = new Plane(-cam.transform.forward, target);

        return plane.Raycast(ray, out float enter)
            ? ray.GetPoint(enter)
            : target + Vector3.up * 10f;
    }
}
