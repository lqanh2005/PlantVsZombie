using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [SerializeField] private float fadeDuration = 0.35f;

    private CanvasGroup fadeGroup;
    private bool isLoading;

    protected override void OnAwake()
    {
        CreateFadeOverlay();
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading)
            return;

        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        isLoading = true;
        fadeGroup.blocksRaycasts = true;

        yield return fadeGroup.DOFade(1f, fadeDuration).SetUpdate(true).WaitForCompletion();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
            yield return null;

        yield return fadeGroup.DOFade(0f, fadeDuration).SetUpdate(true).WaitForCompletion();

        fadeGroup.blocksRaycasts = false;
        isLoading = false;
    }

    private void CreateFadeOverlay()
    {
        GameObject canvasGo = new GameObject("FadeCanvas", typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster));
        canvasGo.transform.SetParent(transform, false);

        Canvas canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        GameObject imageGo = new GameObject("Fade", typeof(RectTransform), typeof(Image));
        imageGo.transform.SetParent(canvasGo.transform, false);

        RectTransform rect = imageGo.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        imageGo.GetComponent<Image>().color = Color.black;

        fadeGroup = canvasGo.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        fadeGroup.interactable = false;
    }
}
