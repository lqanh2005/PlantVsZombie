using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ZombieSetupTool
{
    private const string ScenePath = "Assets/Scenes/GamePlay.unity";
    private const string EnemyBulletPath = "Assets/Prefab/EnemyBullet.prefab";
    private const string CowboyPath = "Assets/Prefab/Earthworm.prefab";
    private const string PiratePath = "Assets/Prefab/Gloom.prefab";
    private const string GolemPath = "Assets/Prefab/Golem Earth.prefab";
    private const string MeleeDataPath = "Assets/Resources/Data/MeleEnemyDataList.asset";
    private const string RangedDataPath = "Assets/Resources/Data/RangedEnemyDataList.asset";
    private const string TankDataPath = "Assets/Resources/Data/TankEnemyDataList.asset";
    private const string WallnutPath = "Assets/Prefab/Wallnut.prefab";

    private static string AutoRunKey => "ZombieSetupTool.Done.v3." + Application.dataPath;

    static ZombieSetupTool()
    {
        if (EditorPrefs.GetBool(AutoRunKey, false))
            return;
        EditorApplication.delayCall += AutoRun;
    }

    private static void AutoRun()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;

        if (SetupAll())
            EditorPrefs.SetBool(AutoRunKey, true);
    }

    [MenuItem("Tools/Game Setup/Setup All")]
    public static void SetupAllMenu()
    {
        SetupAll();
    }

    private static bool SetupAll()
    {
        GameObject bullet = CreateEnemyBulletPrefab();
        SetupZombiePrefab(CowboyPath, null);
        SetupZombiePrefab(PiratePath, bullet);
        SetupZombiePrefab(GolemPath, null);

        LinkData(MeleeDataPath, "meleEnemyDataList", CowboyPath, null);
        LinkData(RangedDataPath, "rangedEnemyDataList", PiratePath, bullet);
        LinkData(TankDataPath, "tankEnemyDataList", GolemPath, null);

        SetupWallnutPrefab();

        AssetDatabase.SaveAssets();
        return SetupScene();
    }

    private static void SetupWallnutPrefab()
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(WallnutPath) == null)
            return;

        GameObject root = PrefabUtility.LoadPrefabContents(WallnutPath);
        try
        {
            if (root.GetComponent<PlantBase>() != null)
                return;

            Wallnut wallnut = root.AddComponent<Wallnut>();
            SerializedObject so = new SerializedObject(wallnut);
            so.FindProperty("plantId").intValue = 1;
            so.FindProperty("plantType").enumValueIndex = (int)PlantType.Defense;
            so.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(root, WallnutPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    private static GameObject CreateEnemyBulletPrefab()
    {
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(EnemyBulletPath);
        if (existing != null)
            return existing;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "EnemyBullet";
        go.transform.localScale = Vector3.one * 0.3f;
        go.GetComponent<SphereCollider>().isTrigger = true;

        Rigidbody body = go.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;

        go.AddComponent<EnemyBullet>();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, EnemyBulletPath);
        Object.DestroyImmediate(go);
        return prefab;
    }

    private static void SetupZombiePrefab(string path, GameObject bullet)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
            return;

        GameObject root = PrefabUtility.LoadPrefabContents(path);
        try
        {
            ZombieBase zombie = root.GetComponentInChildren<ZombieBase>(true);
            GameObject host = zombie != null ? zombie.gameObject : root;

            if (host.GetComponent<Collider>() == null)
            {
                BoxCollider box = host.AddComponent<BoxCollider>();
                FitCollider(box, host);
                box.isTrigger = true;
            }

            if (bullet != null && zombie is RangedEnemyBase)
            {
                SerializedObject so = new SerializedObject(zombie);
                SerializedProperty bulletProperty = so.FindProperty("bulletPrefab");
                if (bulletProperty != null && bulletProperty.objectReferenceValue == null)
                {
                    bulletProperty.objectReferenceValue = bullet;
                    so.ApplyModifiedPropertiesWithoutUndo();
                }
            }

            PrefabUtility.SaveAsPrefabAsset(root, path);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    private static void FitCollider(BoxCollider box, GameObject host)
    {
        Renderer[] renderers = host.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0)
        {
            box.center = Vector3.up;
            box.size = new Vector3(1f, 2f, 1f);
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        Vector3 size = host.transform.InverseTransformVector(bounds.size);
        box.center = host.transform.InverseTransformPoint(bounds.center);
        box.size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
    }

    private static void LinkData(string assetPath, string listName, string prefabPath, GameObject bullet)
    {
        ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (asset == null || prefab == null)
            return;

        SerializedObject so = new SerializedObject(asset);
        SerializedProperty list = so.FindProperty(listName);
        if (list == null)
            return;

        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty element = list.GetArrayElementAtIndex(i);
            if (element.FindPropertyRelative("enemyId").intValue != 1)
                continue;

            SerializedProperty prefabProperty = element.FindPropertyRelative("enemyPrefab");
            if (prefabProperty.objectReferenceValue == null)
                prefabProperty.objectReferenceValue = prefab;

            SerializedProperty bulletProperty = element.FindPropertyRelative("bullet");
            if (bullet != null && bulletProperty != null && bulletProperty.objectReferenceValue == null)
                bulletProperty.objectReferenceValue = bullet;
        }

        if (so.ApplyModifiedPropertiesWithoutUndo())
            EditorUtility.SetDirty(asset);
    }

    private static bool SetupScene()
    {
        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        if (!scene.isLoaded)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return false;
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        PlayerContain playerContain = FindInScene<PlayerContain>(scene);
        if (playerContain == null)
        {
            Debug.LogError($"PlayerContain not found in {ScenePath}.");
            return false;
        }

        ZombieSpawner spawner = playerContain.zombieSpawner != null
            ? playerContain.zombieSpawner
            : FindInScene<ZombieSpawner>(scene);

        if (spawner == null)
        {
            GameObject go = new GameObject("ZombieSpawner");
            SceneManager.MoveGameObjectToScene(go, scene);
            go.transform.SetParent(playerContain.transform, false);
            spawner = go.AddComponent<ZombieSpawner>();
            FillSpawnEntries(spawner);
        }

        SunManager sunManager = playerContain.sunManager != null
            ? playerContain.sunManager
            : FindInScene<SunManager>(scene);

        if (sunManager == null)
        {
            GameObject go = new GameObject("SunManager");
            SceneManager.MoveGameObjectToScene(go, scene);
            go.transform.SetParent(playerContain.transform, false);
            sunManager = go.AddComponent<SunManager>();
        }

        SetupSunText(scene, sunManager);

        playerContain.zombieSpawner = spawner;
        playerContain.sunManager = sunManager;
        EditorUtility.SetDirty(playerContain);
        EditorSceneManager.MarkSceneDirty(scene);
        return EditorSceneManager.SaveScene(scene);
    }

    private static void SetupSunText(Scene scene, SunManager sunManager)
    {
        SerializedObject so = new SerializedObject(sunManager);
        SerializedProperty textProperty = so.FindProperty("sunText");
        if (textProperty.objectReferenceValue != null)
            return;

        Canvas canvas = null;
        PlantList plantList = FindInScene<PlantList>(scene);
        if (plantList != null)
            canvas = plantList.GetComponentInParent<Canvas>(true);
        if (canvas == null)
            canvas = FindInScene<Canvas>(scene);
        if (canvas == null)
            return;
        canvas = canvas.rootCanvas;

        GameObject go = new GameObject("SunText", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(canvas.transform, false);

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -10f);
        rect.sizeDelta = new Vector2(300f, 60f);

        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        text.text = "Sun: 150";
        text.fontSize = 40f;
        text.fontStyle = FontStyles.Bold;
        text.color = new Color(1f, 0.85f, 0.1f);
        text.alignment = TextAlignmentOptions.Center;
        text.outlineWidth = 0.2f;
        text.outlineColor = Color.black;
        text.raycastTarget = false;

        textProperty.objectReferenceValue = text;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void FillSpawnEntries(ZombieSpawner spawner)
    {
        GameObject cowboy = AssetDatabase.LoadAssetAtPath<GameObject>(CowboyPath);
        GameObject pirate = AssetDatabase.LoadAssetAtPath<GameObject>(PiratePath);
        GameObject golem = AssetDatabase.LoadAssetAtPath<GameObject>(GolemPath);

        (GameObject prefab, float delay)[] entries =
        {
            (cowboy, 25f),
            (cowboy, 20f),
            (pirate, 18f),
            (golem, 22f),
            (cowboy, 15f),
            (pirate, 14f),
            (golem, 12f),
        };

        SerializedObject so = new SerializedObject(spawner);
        SerializedProperty list = so.FindProperty("spawnEntries");
        list.arraySize = 0;
        foreach ((GameObject prefab, float delay) in entries)
        {
            if (prefab == null)
                continue;

            int index = list.arraySize;
            list.InsertArrayElementAtIndex(index);
            SerializedProperty element = list.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("zombiePrefab").objectReferenceValue = prefab;
            element.FindPropertyRelative("delay").floatValue = delay;
            element.FindPropertyRelative("row").intValue = -1;
        }
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static T FindInScene<T>(Scene scene) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            T found = root.GetComponentInChildren<T>(true);
            if (found != null)
                return found;
        }
        return null;
    }
}
