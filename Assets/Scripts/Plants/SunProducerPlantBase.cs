using UnityEngine;

public abstract class SunProducerPlantBase : PlantBase
{
    [SerializeField] protected GameObject sunPrefab;
    [SerializeField] protected Transform spawnSun;
    [SerializeField] protected float sunSpawnHeight = 1.5f;
    [SerializeField] protected float firstSunDelay = 5f;
    protected float produceTimer;

    protected SunProducerData SunData => (SunProducerData)data;

    public override void Init()
    {
        base.Init();
        if (!isAlive)
            return;

        produceTimer = Mathf.Min(firstSunDelay, SunData.productionInterval);
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        produceTimer -= Time.deltaTime;

        if (produceTimer <= 0f)
        {
            ProduceSun();
            produceTimer = SunData.productionInterval;
        }
    }

    protected virtual void ProduceSun()
    {
        if (sunPrefab == null)
            return;

        Vector3 basePosition = spawnSun != null ? spawnSun.position : transform.position;
        GameObject sunObject = SimplePool.Spawn(sunPrefab, basePosition + Vector3.up * sunSpawnHeight, Quaternion.identity);

        Sun sun = sunObject.GetComponent<Sun>();
        if (sun != null)
            sun.Init(SunData.sunAmount);
    }
}
