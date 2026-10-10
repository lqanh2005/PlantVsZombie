using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Color lockedTextColor = new Color(1f, 1f, 1f, 0.35f);

    private int levelId;
    private Action<int> onClick;

    public void SetData(int levelId, bool isUnlocked, Action<int> onClick)
    {
        this.levelId = levelId;
        this.onClick = onClick;

        if (levelTxt != null)
        {
            levelTxt.text = levelId.ToString();
            levelTxt.color = isUnlocked ? Color.white : lockedTextColor;
        }

        btn.interactable = isUnlocked;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => this.onClick?.Invoke(this.levelId));
    }
}
