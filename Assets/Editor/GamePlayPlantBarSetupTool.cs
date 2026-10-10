using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class GamePlayPlantBarSetupTool
{
    private const string GamePlayScenePath = "Assets/Scenes/GamePlay.unity";
    private const string HomeScenePath = "Assets/Scenes/Home.unity";
    private const string PlantCardPrefabPath = "Assets/Prefab/PlantCard.prefab";
    private const string SourcePanelName = "SeedBankPanel";
    private const string SourceSlotGridName = "SelectedSeedGrid";
    private const string SourceSunCounterName = "SunCounter";
    private const string PanelName = "PlantBarPanel";
    private const string SlotGridName = "SlotGrid";
    private const string OldSunTextName = "SunText";

    private static string AutoRunKey => "GamePlayPlantBarSetupTool.Done.v1." + Application.dataPath;

    static GamePlayPlantBarSetupTool()
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

    [MenuItem("Tools/Game Setup/Setup GamePlay Plant Bar")]
    public static void SetupMenu()
    {
        Setup();
    }

    private static bool Setup()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return false;

        Scene gamePlay = EditorSceneManager.OpenScene(GamePlayScenePath, OpenSceneMode.Single);

        Canvas canvas = FindCanvas(gamePlay);
        if (canvas == null)
        {
            Debug.LogError($"Canvas not found in {GamePlayScenePath}.");
            return false;
        }

        Transform existing = canvas.transform.Find(PanelName);
        GameObject panel = existing != null ? existing.gameObject : CopyPanelFromHome(gamePlay, canvas);
        if (panel == null)
            return false;

        Transform slotGrid = FindDeep(panel.transform, SlotGridName);
        TMP_Text sunText = FindSunText(panel.transform);

        PlantList oldList = FindInScene<PlantList>(gamePlay, p => p.gameObject != panel);
        if (oldList != null)
            Object.DestroyImmediate(oldList.gameObject);

        PlantList plantList = panel.GetComponent<PlantList>();
        if (plantList == null)
            plantList = panel.AddComponent<PlantList>();

        SerializedObject listSo = new SerializedObject(plantList);
        listSo.FindProperty("slotContainer").objectReferenceValue = slotGrid;
        listSo.FindProperty("cardPrefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<PlantCard>(PlantCardPrefabPath);
        listSo.ApplyModifiedPropertiesWithoutUndo();

        GameScene gameScene = FindInScene<GameScene>(gamePlay, null);
        if (gameScene != null)
        {
            SerializedObject sceneSo = new SerializedObject(gameScene);
            sceneSo.FindProperty("plantList").objectReferenceValue = plantList;
            sceneSo.ApplyModifiedPropertiesWithoutUndo();
        }

        SunManager sunManager = FindInScene<SunManager>(gamePlay, null);
        if (sunManager != null && sunText != null)
        {
            SerializedObject sunSo = new SerializedObject(sunManager);
            SerializedProperty textProperty = sunSo.FindProperty("sunText");
            Object oldText = textProperty.objectReferenceValue;

            textProperty.objectReferenceValue = sunText;
            sunSo.FindProperty("sunFormat").stringValue = "{0}";
            sunSo.ApplyModifiedPropertiesWithoutUndo();

            if (oldText is TMP_Text oldTmp && oldTmp != sunText && oldTmp.name == OldSunTextName)
                Object.DestroyImmediate(oldTmp.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(gamePlay);
        return EditorSceneManager.SaveScene(gamePlay);
    }

    private static GameObject CopyPanelFromHome(Scene gamePlay, Canvas canvas)
    {
        Scene home = EditorSceneManager.OpenScene(HomeScenePath, OpenSceneMode.Additive);
        try
        {
            GameObject source = null;
            foreach (GameObject root in home.GetRootGameObjects())
            {
                Transform found = FindDeep(root.transform, SourcePanelName);
                if (found != null)
                {
                    source = found.gameObject;
                    break;
                }
            }

            if (source == null)
            {
                Debug.LogError($"{SourcePanelName} not found in {HomeScenePath}.");
                return null;
            }

            GameObject panel = Object.Instantiate(source);
            SceneManager.MoveGameObjectToScene(panel, gamePlay);
            panel.name = PanelName;
            panel.SetActive(true);
            panel.transform.SetParent(canvas.transform, false);
            panel.transform.SetAsFirstSibling();

            Transform slotGrid = FindDeep(panel.transform, SourceSlotGridName);
            if (slotGrid != null)
            {
                slotGrid.name = SlotGridName;
                foreach (Transform slot in slotGrid)
                {
                    if (slot.GetComponent<SlotCardUI>() == null)
                        slot.gameObject.AddComponent<SlotCardUI>();
                }
            }

            return panel;
        }
        finally
        {
            EditorSceneManager.CloseScene(home, true);
        }
    }

    private static TMP_Text FindSunText(Transform panel)
    {
        Transform sunCounter = FindDeep(panel, SourceSunCounterName);
        return sunCounter != null ? sunCounter.GetComponentInChildren<TMP_Text>(true) : null;
    }

    private static Canvas FindCanvas(Scene scene)
    {
        Canvas canvas = FindInScene<Canvas>(scene, null);
        return canvas != null ? canvas.rootCanvas : null;
    }

    private static Transform FindDeep(Transform root, string name)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == name)
                return t;
        }
        return null;
    }

    private static T FindInScene<T>(Scene scene, System.Func<T, bool> predicate) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (T found in root.GetComponentsInChildren<T>(true))
            {
                if (predicate == null || predicate(found))
                    return found;
            }
        }
        return null;
    }
}
