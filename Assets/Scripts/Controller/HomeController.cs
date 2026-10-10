using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeController : Singleton<HomeController>
{
    public List<SelectedPlantData> selectedPlants = new();
    public SelectedSeedGrid selectedSeedGrid;
    public PlantGridUI plantGridUI;

    [SerializeField] private Button startBtn;
    [SerializeField] private Button letsRockBtn;
    [SerializeField] private LevelSelectionPopup levelSelectionPopup;
    [SerializeField] private GameObject plantSelectionRoot;

    private bool isPlantGridReady;

    private void Start()
    {
        if (plantSelectionRoot != null)
            plantSelectionRoot.SetActive(false);
        if (levelSelectionPopup != null)
            levelSelectionPopup.Hide();

        if (startBtn != null)
        {
            startBtn.gameObject.SetActive(true);
            startBtn.onClick.AddListener(OnStartClicked);
        }

        if (letsRockBtn != null)
            letsRockBtn.onClick.AddListener(OnLetsRockClicked);
    }

    public void OnStartClicked()
    {
        if (levelSelectionPopup != null)
            levelSelectionPopup.Show(OnLevelSelected);
    }

    private void OnLevelSelected(int levelId)
    {
        UseProfile.CurrentLevel = levelId;

        if (startBtn != null)
            startBtn.gameObject.SetActive(false);
        if (plantSelectionRoot != null)
            plantSelectionRoot.SetActive(true);

        if (!isPlantGridReady)
        {
            plantGridUI.Init();
            isPlantGridReady = true;
        }
    }

    public void OnLetsRockClicked()
    {
        if (selectedPlants.Count == 0)
            return;

        UseProfile.SaveSelectedPlants(selectedPlants);

        SceneLoader.Load(SceneLoader.GamePlay);
    }
}
