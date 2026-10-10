using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public PlantList plantList;
    public GameObject losePopup;
    public Button loseBtn;
    public GameObject winPopup;
    public Button winBtn;
    public void Init()
    {
        plantList.Init();
        loseBtn.onClick.AddListener(() =>
        {
            losePopup.SetActive(false);
            // Restart the game or load the main menu scene
        });
    }
}
