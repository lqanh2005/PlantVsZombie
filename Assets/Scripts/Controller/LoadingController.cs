using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text progressTxt;
    [SerializeField] private float minDuration = 2f;

    private IEnumerator Start()
    {
        Time.timeScale = 1f;
        SetProgress(0f);

        PreloadData();

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneLoader.Home);
        operation.allowSceneActivation = false;

        float timer = 0f;
        float progress = 0f;
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);
            progress = Mathf.Min(timer / minDuration, loadProgress);
            SetProgress(progress);
            yield return null;
        }

        UseProfile.FirstLoading = true;
        operation.allowSceneActivation = true;
    }

    private void PreloadData()
    {
        _ = PlantDatabase.Instance;
        _ = EnemyDataBase.Instance;
        _ = EffectDataList.Instance;
        _ = LevelDataList.Instance;
    }

    private void SetProgress(float value)
    {
        if (progressFill != null)
            progressFill.fillAmount = value;
        if (progressTxt != null)
            progressTxt.text = $"{Mathf.RoundToInt(value * 100f)}%";
    }
}
