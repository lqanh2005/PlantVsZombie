using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantCard : MonoBehaviour
{
    public int plantId;
    public Button btn;
    public Image plantImage;
    public TMP_Text sunTxt;
    public PlantType plantType;
    public GameObject plantPrefab;
    public void Init(int id, Sprite sprite, int sunCost)
    {
        plantId = id;
        plantImage.sprite = sprite;
        sunTxt.text = sunCost.ToString();
        btn.onClick.AddListener(() =>
        {
            GamePlayController.Instance.playerContain.plantSpawner.SelectPlant(plantPrefab);
        });
    }
    public void Init()
    {
        PlantData data = PlantDatabase.Instance.GetPlantDataByType(plantType, plantId);
        this.plantImage.sprite = data.icon;
        this.sunTxt.text = data.sunCost.ToString();
        this.plantPrefab = data.plantPrefab;
        btn.onClick.AddListener(() =>
        {
            GamePlayController.Instance.playerContain.plantSpawner.SelectPlant(plantPrefab);
        });
    }

}
