using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataList", menuName = "Level/LevelDataList")]
public class LevelDataList : SingletonScriptableObject<LevelDataList>
{
    public List<LevelData> levelDataList = new List<LevelData>();

    public LevelData GetLevelData(int levelId)
    {
        LevelData result = levelDataList.Find(d => d.levelId == levelId);
        if (result == null)
            Debug.LogError($"Level data not found: level {levelId}.");
        return result;
    }
}

[System.Serializable]
public class LevelData
{
    public int levelId;
    public List<WaveData> waves = new List<WaveData>();
}

[System.Serializable]
public class WaveData
{
    [Tooltip("Thời gian chờ trước khi wave bắt đầu (tính từ lúc wave trước spawn xong)")]
    public float startDelay = 10f;
    [Tooltip("Bắt đầu wave sớm nếu trên sân không còn zombie")]
    public bool startWhenCleared = true;

    public int zombieCount = 5;
    public float spawnInterval = 3f;
    [Tooltip("Spawn nhiều con cùng lúc mỗi lượt")]
    public int spawnPerTick = 1;

    [Tooltip("Các hàng được phép spawn, để trống = ngẫu nhiên mọi hàng")]
    public List<int> rows = new List<int>();

    public List<ZombieSpawnRate> zombies = new List<ZombieSpawnRate>();
}

[System.Serializable]
public class ZombieSpawnRate
{
    public EnemyType enemyType;
    public int enemyId;
    [Tooltip("Trọng số tỉ lệ xuất hiện, tỉ lệ = weight / tổng weight của wave")]
    [Min(0f)] public float weight = 1f;
}
