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
public static class HomeSetupTool
{
    private const string HomeScenePath = "Assets/Scenes/Home.unity";
    private const string GamePlayScenePath = "Assets/Scenes/GamePlay.unity";
    private const string PlantSelectionRootName = "PlantSelectionRoot";
    private const string StartButtonName = "StartButton";
    private const string PopupName = "LevelSelectionPopup";

    private static string AutoRunKey => "HomeSetupTool.Done.v1." + Application.dataPath;

    static HomeSetupTool()
    {
        if (EditorPrefs.GetBool(AutoRunKey, false))
            return;
        EditorApplication.delayCall += AutoRun;
    }

    private static void AutoRun()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;

        if (SetupHomeScene())
            EditorPrefs.SetBool(AutoRunKey, true);
    }

    [MenuItem("Tools/Game Setup/Setup Home Scene")]
    public static void SetupHomeSceneMenu()
    {
        SetupHomeScene();
    }

    private static bool SetupHomeScene()
    {
        LoadingSetupTool.SetupBuildSettings();

        Scene scene = SceneManager.GetSceneByPath(HomeScenePath);
        if (!scene.isLoaded)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return false;
            scene = EditorSceneManager.OpenScene(HomeScenePath, OpenSceneMode.Single);
        }

        HomeController home = FindInScene<HomeController>(scene);
        if (home == null)
        {
            Debug.LogError($"HomeController not found in {HomeScenePath}.");
            return false;
        }

        Canvas canvas = FindInScene<Canvas>(scene);
        if (canvas == null)
        {
            Debug.LogError($"Canvas not found in {HomeScenePath}.");
            return false;
        }
        canvas = canvas.rootCanvas;

        GameObject plantSelectionRoot = SetupPlantSelectionRoot(canvas);
        Button startBtn = SetupStartButton(canvas);
        LevelSelectionPopup popup = SetupPopup(canvas);
        Button letsRockBtn = FindLetsRockButton(plantSelectionRoot.transform);

        SerializedObject so = new SerializedObject(home);
        so.FindProperty("m_DontDestroyOnLoad").boolValue = false;
        so.FindProperty("startBtn").objectReferenceValue = startBtn;
        so.FindProperty("letsRockBtn").objectReferenceValue = letsRockBtn;
        so.FindProperty("levelSelectionPopup").objectReferenceValue = popup;
        so.FindProperty("plantSelectionRoot").objectReferenceValue = plantSelectionRoot;
        so.ApplyModifiedPropertiesWithoutUndo();

        if (letsRockBtn == null)
            Debug.LogWarning("HomeSetupTool: LET'S ROCK button not found, assign it on HomeController manually.");

        EditorUtility.SetDirty(home);
        EditorSceneManager.MarkSceneDirty(scene);
        return EditorSceneManager.SaveScene(scene);
    }

    private static GameObject SetupPlantSelectionRoot(Canvas canvas)
    {
        Transform existing = canvas.transform.Find(PlantSelectionRootName);
        if (existing != null)
            return existing.gameObject;

        List<Transform> children = new List<Transform>();
        foreach (Transform child in canvas.transform)
            children.Add(child);

        GameObject root = CreateUIObject(PlantSelectionRootName, canvas.transform);
        Stretch(root.GetComponent<RectTransform>());

        foreach (Transform child in children)
            child.SetParent(root.transform, false);

        return root;
    }

    private static Button SetupStartButton(Canvas canvas)
    {
        Transform existing = canvas.transform.Find(StartButtonName);
        if (existing != null)
            return existing.GetComponent<Button>();

        Button button = CreateButton(StartButtonName, canvas.transform, "START", 56f);
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(420f, 120f);
        return button;
    }

    private static LevelSelectionPopup SetupPopup(Canvas canvas)
    {
        Transform existing = canvas.transform.Find(PopupName);
        if (existing != null)
            return existing.GetComponent<LevelSelectionPopup>();

        GameObject popupGo = CreateUIObject(PopupName, canvas.transform, typeof(Image));
        Stretch(popupGo.GetComponent<RectTransform>());
        popupGo.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.7f);

        GameObject panel = CreateUIObject("Panel", popupGo.transform, typeof(Image));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(900f, 620f);
        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = PanelColor;
        panelImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        panelImage.type = Image.Type.Sliced;

        TextMeshProUGUI title = CreateText("Title", panel.transform, "LEVEL SELECTION", 56f);
        RectTransform titleRect = title.rectTransform;
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -20f);
        titleRect.sizeDelta = new Vector2(0f, 90f);

        GameObject content = CreateUIObject("Content", panel.transform, typeof(GridLayoutGroup));
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(40f, 40f);
        contentRect.offsetMax = new Vector2(-40f, -130f);
        GridLayoutGroup grid = content.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(180f, 180f);
        grid.spacing = new Vector2(40f, 40f);
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;

        Button templateBtn = CreateButton("LevelButtonTemplate", content.transform, "1", 72f);
        LevelButtonUI template = templateBtn.gameObject.AddComponent<LevelButtonUI>();
        SerializedObject templateSo = new SerializedObject(template);
        templateSo.FindProperty("btn").objectReferenceValue = templateBtn;
        templateSo.FindProperty("levelTxt").objectReferenceValue = templateBtn.GetComponentInChildren<TextMeshProUGUI>(true);
        templateSo.ApplyModifiedPropertiesWithoutUndo();
        templateBtn.gameObject.SetActive(false);

        Button closeBtn = CreateButton("CloseButton", panel.transform, "X", 48f);
        RectTransform closeRect = closeBtn.GetComponent<RectTransform>();
        closeRect.anchorMin = closeRect.anchorMax = new Vector2(1f, 1f);
        closeRect.pivot = new Vector2(1f, 1f);
        closeRect.anchoredPosition = new Vector2(-20f, -20f);
        closeRect.sizeDelta = new Vector2(80f, 80f);
        closeBtn.GetComponent<Image>().color = new Color(0.75f, 0.2f, 0.15f, 1f);

        LevelSelectionPopup popup = popupGo.AddComponent<LevelSelectionPopup>();
        SerializedObject so = new SerializedObject(popup);
        so.FindProperty("content").objectReferenceValue = content.transform;
        so.FindProperty("levelButtonTemplate").objectReferenceValue = template;
        so.FindProperty("closeBtn").objectReferenceValue = closeBtn;
        so.ApplyModifiedPropertiesWithoutUndo();

        popupGo.SetActive(false);
        return popup;
    }

    private static Button FindLetsRockButton(Transform root)
    {
        foreach (TMP_Text text in root.GetComponentsInChildren<TMP_Text>(true))
        {
            if (!text.text.ToUpperInvariant().Contains("ROCK"))
                continue;

            Button button = text.GetComponentInParent<Button>(true);
            if (button != null)
                return button;
        }
        return null;
    }

}
