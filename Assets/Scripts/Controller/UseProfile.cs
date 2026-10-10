using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.UIElements;

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

        string json = PlayerPrefs.GetString(
            StringHelper.SELECTED_PLANTS
        );

        SelectedPlantList data =
            JsonUtility.FromJson<SelectedPlantList>(json);

        return data?.plants ?? new List<SelectedPlantData>();
    }

    //public static int CurrentLevel
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1);
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL, value);
    //        PlayerPrefs.Save();
    //    }
    //}



    //public static int Atom_Booster
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt(StringHelper.ATOM_BOOSTER, 3);
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.ATOM_BOOSTER, value);
    //        PlayerPrefs.Save();
    //        EventDispatcher.EventDispatcher.Instance.PostEvent(EventID.CHANGE_ATOM_BOOSTER);
    //    }
    //}
    //public bool OnVibration
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt(StringHelper.ONOFF_VIBRATION, 1) == 1;
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.ONOFF_VIBRATION, value ? 1 : 0);
    //        MMVibrationManager.SetHapticsActive(value);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public bool OnSound
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt(StringHelper.ONOFF_SOUND, 1) == 1;
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.ONOFF_SOUND, value ? 1 : 0);
    //        GameController.Instance.musicManager.SetSoundVolume(value ? 1 : 0);
    //        PlayerPrefs.Save();
    //    }
    //}
    //public bool OnMusic
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt(StringHelper.ONOFF_MUSIC, 1) == 1;
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.ONOFF_MUSIC, value ? 1 : 0);
    //        GameController.Instance.musicManager.SetMusicVolume(value ? 0.15f : 0);
    //        PlayerPrefs.Save();
    //    }
    //}
}

