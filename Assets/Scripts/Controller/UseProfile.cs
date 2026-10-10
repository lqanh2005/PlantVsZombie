using System.Collections.Generic;
using UnityEngine;

public class UseProfile : MonoBehaviour
{
    public static bool FirstLoading
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.LOADING_COMPLETE, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.LOADING_COMPLETE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static void SaveSelectedPlants(List<SelectedPlantData> plants)
    {
        SelectedPlantList data = new SelectedPlantList
        {
            plants = plants
        };

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(StringHelper.SELECTED_PLANTS, json);
        PlayerPrefs.Save();
    }

    public static List<SelectedPlantData> LoadSelectedPlants()
    {
        if (!PlayerPrefs.HasKey(StringHelper.SELECTED_PLANTS))
            return new List<SelectedPlantData>();

        string json = PlayerPrefs.GetString(StringHelper.SELECTED_PLANTS);
        SelectedPlantList data = JsonUtility.FromJson<SelectedPlantList>(json);

        return data?.plants ?? new List<SelectedPlantData>();
    }

    public static int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL, value);
            PlayerPrefs.Save();
        }
    }

    public static int MaxUnlockedLevel
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.MAX_UNLOCKED_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.MAX_UNLOCKED_LEVEL, value);
            PlayerPrefs.Save();
        }
    }

    public static void UnlockLevel(int levelId)
    {
        if (levelId > MaxUnlockedLevel)
            MaxUnlockedLevel = levelId;
    }
}
