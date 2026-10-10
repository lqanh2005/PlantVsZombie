using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public PlantList plantList;
    public GameObject losePopup;
    public Button loseBtn;
    public GameObject winPopup;
    public Button winBtn;

    [Header("Wave")]
    public TMP_Text waveTxt;

    [Header("Pause")]
    public Button pauseBtn;
    public GameObject pausePopup;
    public Button resumeBtn;
    public Button pauseHomeBtn;

    public void Init()
    {
        plantList.Init();

        if (losePopup != null)
            losePopup.SetActive(false);
        if (winPopup != null)
            winPopup.SetActive(false);
        if (pausePopup != null)
            pausePopup.SetActive(false);

        AddListener(loseBtn, GamePlayController.Instance.BackToHome);
        AddListener(winBtn, GamePlayController.Instance.BackToHome);
        AddListener(pauseBtn, GamePlayController.Instance.Pause);
        AddListener(resumeBtn, GamePlayController.Instance.Resume);
        AddListener(pauseHomeBtn, GamePlayController.Instance.BackToHome);
    }

    private void AddListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.AddListener(action);
    }

    public void ShowWin()
    {
        SetPauseButtonVisible(false);
        if (winPopup != null)
            winPopup.SetActive(true);
    }

    public void ShowLose()
    {
        SetPauseButtonVisible(false);
        if (losePopup != null)
            losePopup.SetActive(true);
    }

    public void SetPausePopup(bool isVisible)
    {
        if (pausePopup != null)
            pausePopup.SetActive(isVisible);
    }

    public void UpdateWave(int level, int currentWave, int totalWaves)
    {
        if (waveTxt != null)
            waveTxt.text = $"Level {level} - Wave {currentWave}/{totalWaves}";
    }

    private void SetPauseButtonVisible(bool isVisible)
    {
        if (pauseBtn != null)
            pauseBtn.gameObject.SetActive(isVisible);
    }
}
