using System;
using UnityEngine;

public class SunProducerPlantBase : PlantBase
{
    [SerializeField] private SunProducerData sunProducerData;
    [SerializeField] private GameObject sunPrefab;
    [SerializeField] private GameObject spawnSun;
    private float activeTimer;

    public override void Init()
    {
        base.Init();
        sunProducerData = (SunProducerData)PlantDatabase.Instance.GetPlantDataByType(plantType, plantId - 1);
        currentHealth = sunProducerData.health;
        activeTimer = sunProducerData.productionInterval; // Initialize the timer
    }
    protected virtual void Update()
    {
        if (!isAlive)
            return;

        activeTimer -= Time.deltaTime;

        if (activeTimer <= 0f)
        {
            ProduceSun();
            activeTimer = sunProducerData.productionInterval;
        }
    }

    private void ProduceSun()
    {
        SimplePool.Spawn(
            sunPrefab,
            spawnSun.transform.position + Vector3.up * 1.5f, // Adjust the position as needed
            Quaternion.identity
        );
    }
}
