using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EditorUIBuilder;

[InitializeOnLoad]
public static class LoadingSetupTool
{
    public const string LoadingScenePath = "Assets/Scenes/Loading.unity";
    public const string HomeScenePath = "Assets/Scenes/Home.unity";
    public const string GamePlayScenePath = "Assets/Scenes/GamePlay.unity";
    private const string CanvasName = "LoadingCanvas";

    private static string AutoRunKey => "LoadingSetupTool.Done.v1." + Application.dataPath;

    static LoadingSetupTool()
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

    [MenuItem("Tools/Game Setup/Setup Loading Scene")]
    public static void SetupMenu()
    {
        Setup();
    }

    public static void SetupBuildSettings()
    {
        string[] ordered = { LoadingScenePath, HomeScenePath, GamePlayScenePath };
        List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes
            .Where(s => !ordered.Contains(s.path))
            .ToList();
        scenes.InsertRange(0, ordered.Select(path => new EditorBuildSettingsScene(path, true)));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static bool Setup()
    {
        SetupBuildSettings();

        Scene scene = SceneManager.GetSceneByPath(LoadingScenePath);
        if (!scene.isLoaded)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return false;
            scene = EditorSceneManager.OpenScene(LoadingScenePath, OpenSceneMode.Single);
        }

        if (FindInScene<LoadingController>(scene) != null)
            return true;

        GameObject canvasGo = new GameObject(CanvasName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        SceneManager.MoveGameObjectToScene(canvasGo, scene);
        canvasGo.layer = LayerMask.NameToLayer("UI");

        Canvas canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject background = CreateUIObject("Background", canvasGo.transform, typeof(Image));
        Stretch(background.GetComponent<RectTransform>());
        background.GetComponent<Image>().color = new Color(0.18f, 0.35f, 0.15f, 1f);

        TextMeshProUGUI title = CreateText("Title", canvasGo.transform, "PLANTS VS ZOMBIES", 110f);
        title.color = new Color(1f, 0.85f, 0.1f);
        Place(title.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 150f), new Vector2(1600f, 200f));

        GameObject barBg = CreateUIObject("ProgressBar", canvasGo.transform, typeof(Image));
        Place(barBg.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0f, -120f), new Vector2(1000f, 50f));
        Image barBgImage = barBg.GetComponent<Image>();
        barBgImage.color = new Color(0f, 0f, 0f, 0.6f);
        barBgImage.sprite = DefaultSprite;
        barBgImage.type = Image.Type.Sliced;

        GameObject fill = CreateUIObject("Fill", barBg.transform, typeof(Image));
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        Stretch(fillRect);
        fillRect.offsetMin = new Vector2(6f, 6f);
        fillRect.offsetMax = new Vector2(-6f, -6f);
        Image fillImage = fill.GetComponent<Image>();
        fillImage.color = new Color(1f, 0.8f, 0.1f, 1f);
        fillImage.sprite = DefaultSprite;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        fillImage.fillAmount = 0f;

        TextMeshProUGUI progressTxt = CreateText("ProgressText", canvasGo.transform, "0%", 44f);
        Place(progressTxt.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, -200f), new Vector2(400f, 70f));

        GameObject controllerGo = new GameObject("LoadingController");
        SceneManager.MoveGameObjectToScene(controllerGo, scene);
        LoadingController controller = controllerGo.AddComponent<LoadingController>();

        SerializedObject so = new SerializedObject(controller);
        so.FindProperty("progressFill").objectReferenceValue = fillImage;
        so.FindProperty("progressTxt").objectReferenceValue = progressTxt;
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(scene);
        return EditorSceneManager.SaveScene(scene);
    }
}
