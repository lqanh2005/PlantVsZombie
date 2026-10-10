using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionPopup : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private LevelButtonUI levelButtonTemplate;
    [SerializeField] private Button closeBtn;

    private readonly List<LevelButtonUI> levelButtons = new();
    private Action<int> onLevelSelected;

    private void Awake()
    {
        if (levelButtonTemplate != null)
            levelButtonTemplate.gameObject.SetActive(false);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(Hide);
    }

    public void Show(Action<int> onLevelSelected)
    {
        this.onLevelSelected = onLevelSelected;
        BuildButtons();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void BuildButtons()
    {
        foreach (LevelButtonUI button in levelButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        levelButtons.Clear();

        LevelDataList levelDataList = LevelDataList.Instance;
        if (levelDataList == null || levelButtonTemplate == null)
            return;

        int maxUnlockedLevel = UseProfile.MaxUnlockedLevel;
        foreach (LevelData levelData in levelDataList.levelDataList)
        {
            LevelButtonUI button = Instantiate(levelButtonTemplate, content, false);
            button.gameObject.SetActive(true);
            button.SetData(levelData.levelId, levelData.levelId <= maxUnlockedLevel, OnLevelClicked);
            levelButtons.Add(button);
        }
    }

    private void OnLevelClicked(int levelId)
    {
        Hide();
        onLevelSelected?.Invoke(levelId);
    }
}
