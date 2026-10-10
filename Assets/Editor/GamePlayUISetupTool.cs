using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EditorUIBuilder;

[InitializeOnLoad]
public static class GamePlayUISetupTool
{
    private const string GamePlayScenePath = "Assets/Scenes/GamePlay.unity";
    private const string WaveTextName = "WaveText";
    private const string PauseButtonName = "PauseButton";
    private const string PausePopupName = "PausePopup";

    private static string AutoRunKey => "GamePlayUISetupTool.Done.v1." + Application.dataPath;

    static GamePlayUISetupTool()
    {
        if (EditorPrefs.GetBool(AutoRunKey, false))
            return;
        EditorApplication.delayCall += AutoRun;
    }

    private static void AutoRun()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;

        if (Setup())
            EditorPrefs.SetBool(AutoRunKey, true);
    }

    [MenuItem("Tools/Game Setup/Setup GamePlay UI")]
    public static void SetupMenu()
    {
        Setup();
    }

    private static bool Setup()
    {
        Scene scene = SceneManager.GetSceneByPath(GamePlayScenePath);
        if (!scene.isLoaded)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return false;
            scene = EditorSceneManager.OpenScene(GamePlayScenePath, OpenSceneMode.Single);
        }

        GameScene gameScene = FindInScene<GameScene>(scene);
        Canvas canvas = FindInScene<Canvas>(scene);
        if (gameScene == null || canvas == null)
        {
            Debug.LogError($"GameScene or Canvas not found in {GamePlayScenePath}.");
            return false;
        }
        Transform root = canvas.rootCanvas.transform;

        TMP_Text waveTxt = SetupWaveText(root);
        Button pauseBtn = SetupPauseButton(root);
        GameObject pausePopup = SetupPausePopup(root, out Button resumeBtn, out Button homeBtn);

        SerializedObject so = new SerializedObject(gameScene);
        so.FindProperty("waveTxt").objectReferenceValue = waveTxt;
        so.FindProperty("pauseBtn").objectReferenceValue = pauseBtn;
        so.FindProperty("pausePopup").objectReferenceValue = pausePopup;
        so.FindProperty("resumeBtn").objectReferenceValue = resumeBtn;
        so.FindProperty("pauseHomeBtn").objectReferenceValue = homeBtn;
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(gameScene);
        EditorSceneManager.MarkSceneDirty(scene);
        return EditorSceneManager.SaveScene(scene);
    }

    private static TMP_Text SetupWaveText(Transform root)
    {
        Transform existing = root.Find(WaveTextName);
        if (existing != null)
            return existing.GetComponent<TMP_Text>();

        TextMeshProUGUI text = CreateText(WaveTextName, root, "Level 1 - Wave 1/1", 40f);
        text.alignment = TextAlignmentOptions.Right;
        Place(text.rectTransform, new Vector2(1f, 1f), new Vector2(-140f, -35f), new Vector2(560f, 60f));
        return text;
    }

    private static Button SetupPauseButton(Transform root)
    {
        Transform existing = root.Find(PauseButtonName);
        if (existing != null)
            return existing.GetComponent<Button>();

        Button button = CreateButton(PauseButtonName, root, "II", 48f);
        Place((RectTransform)button.transform, new Vector2(1f, 1f), new Vector2(-25f, -20f), new Vector2(90f, 90f));
        return button;
    }

    private static GameObject SetupPausePopup(Transform root, out Button resumeBtn, out Button homeBtn)
    {
        Transform existing = root.Find(PausePopupName);
        if (existing != null)
        {
            resumeBtn = existing.Find("Panel/ResumeButton")?.GetComponent<Button>();
            homeBtn = existing.Find("Panel/HomeButton")?.GetComponent<Button>();
            return existing.gameObject;
        }

        GameObject popup = CreateDimBackground(PausePopupName, root);
        GameObject panel = CreatePanel("Panel", popup.transform, new Vector2(600f, 480f));

        TextMeshProUGUI title = CreateText("Title", panel.transform, "PAUSED", 64f);
        Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0f, -30f), new Vector2(500f, 100f));

        resumeBtn = CreateButton("ResumeButton", panel.transform, "RESUME", 44f);
        Place((RectTransform)resumeBtn.transform, new Vector2(0.5f, 0.5f), new Vector2(0f, -10f), new Vector2(360f, 100f));

        homeBtn = CreateButton("HomeButton", panel.transform, "HOME", 44f);
        Place((RectTransform)homeBtn.transform, new Vector2(0.5f, 0.5f), new Vector2(0f, -140f), new Vector2(360f, 100f));

        popup.transform.SetAsLastSibling();
        popup.SetActive(false);
        return popup;
    }
}
