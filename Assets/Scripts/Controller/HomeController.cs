using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : Singleton<HomeController>
{
    public List<SelectedPlantData> selectedPlants = new();
    public SelectedSeedGrid selectedSeedGrid;
    public PlantGridUI plantGridUI;
    private void Start()
    {
        plantGridUI.Init();
    }
    public void OnLetsRockClicked()
    {
        UseProfile.SaveSelectedPlants(selectedPlants);

        SceneManager.LoadScene("GameScene");
    }
}
