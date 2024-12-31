using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptsFinder : EditorWindow
{
    private struct MissingScriptEntry
    {
        public string assetPath;
        public string objectName;

        public MissingScriptEntry(string assetPath, string objectName)
        {
            this.assetPath = assetPath;
            this.objectName = objectName;
        }

        public override bool Equals(object obj)
        {
            if (obj is MissingScriptEntry other)
            {
                return assetPath == other.assetPath && objectName == other.objectName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (assetPath + objectName).GetHashCode();
        }
    }

    private List<MissingScriptEntry> missingScriptEntries = new List<MissingScriptEntry>();
    private List<MissingScriptEntry> ignoreList = new List<MissingScriptEntry>();
    private int ignoredItemsFound = 0;
    private Vector2 scrollPosition;
    private bool viewIgnoreList = false;
    private bool keepIgnoreList = true;
    private bool skipTestingScenes = true;
    private bool skipTrashScenes = true;
    private bool skipTestPrefabs = true;

    private const string IgnoreListKey = "MissingScriptsIgnoreList";

    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptsFinder>("Missing Scripts Finder");
    }

    private void OnEnable()
    {
        LoadIgnoreList();
    }

    private void OnDisable()
    {
        if (keepIgnoreList)
        {
            SaveIgnoreList();
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }

        if (GUILayout.Button(viewIgnoreList ? $"View Main List ({missingScriptEntries.Count})" : $"View Ignore List ({ignoreList.Count})"))
        {
            viewIgnoreList = !viewIgnoreList;
        }

        if (viewIgnoreList)
        {
            DisplayIgnoreList();
        }
        else
        {
            DisplayMissingScriptsList();
        }

        // Bottom UI Layout
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();

        // Bottom-left: Ignored items found
        GUILayout.Label($"Ignored items found: {ignoredItemsFound}", GUILayout.Width(180));
        GUILayout.FlexibleSpace();
        // Bottom-center: Skip Testing Scenes toggle
        skipTestingScenes = EditorGUILayout.Toggle("Skip Testing Scenes", skipTestingScenes, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        // Bottom-center-right: Skip Trash Scenes toggle
        skipTrashScenes = EditorGUILayout.Toggle("Skip Trash Scenes", skipTrashScenes, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        // Bottom-right: KeepIgnoreList toggle
        keepIgnoreList = EditorGUILayout.Toggle("Keep Ignore List", keepIgnoreList, GUILayout.Width(200));

        EditorGUILayout.EndHorizontal();
    }
    private void DisplayMissingScriptsList()
    {
        if (missingScriptEntries.Count > 0)
        {
            EditorGUILayout.LabelField($"Results ({missingScriptEntries.Count})", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < missingScriptEntries.Count; i++)
            {
                var entry = missingScriptEntries[i];
                EditorGUILayout.BeginHorizontal();

                // Adaptive width for the label showing the asset path and object name
                GUILayout.Label($"{entry.assetPath} -> {entry.objectName}", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                {
                    if (entry.assetPath.EndsWith(".prefab"))
                    {
                        OpenPrefabAndSelect(entry.assetPath, entry.objectName);
                    }
                    else if (entry.assetPath.EndsWith(".unity"))
                    {
                        OpenSceneAndSelect(entry.assetPath, entry.objectName);
                    }
                }

                if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                {
                    ignoreList.Add(entry);
                    missingScriptEntries.RemoveAt(i);
                    i--; // Adjust index after removal
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("No missing scripts found.");
        }
    }

    private void DisplayIgnoreList()
    {
        EditorGUILayout.BeginHorizontal();

        // Ignore List Title
        EditorGUILayout.LabelField($"Ignore List ({ignoreList.Count})", EditorStyles.boldLabel);

        // Clear Ignore List Button
        if (GUILayout.Button("Clear", GUILayout.Width(80)))
        {
            if (EditorUtility.DisplayDialog("Clear Ignore List", "Are you sure you want to clear the ignore list?", "Yes", "No"))
            {
                ignoreList.Clear();
            }
        }

        EditorGUILayout.EndHorizontal();

        if (ignoreList.Count > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < ignoreList.Count; i++)
            {
                var entry = ignoreList[i];
                EditorGUILayout.BeginHorizontal();

                // Adaptive width for the label showing the asset path and object name
                GUILayout.Label($"{entry.assetPath} -> {entry.objectName}", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    missingScriptEntries.Add(entry);
                    ignoreList.RemoveAt(i);
                    i--; // Adjust index after removal
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.LabelField("No items in the ignore list.");
        }
    }

    private void FindMissingScripts()
    {
        missingScriptEntries.Clear();
        ignoredItemsFound = 0;

        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssets)
        {
            // Skip package assets
            if (assetPath.StartsWith("Packages/"))
                continue;

            if (skipTestingScenes && assetPath.StartsWith("Assets/Scenes/Testing/"))
                continue;

            if (skipTrashScenes && assetPath.StartsWith("Assets/Scenes/Trash/"))
                continue;

            if (assetPath.EndsWith(".prefab"))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (obj != null)
                {
                    CheckForMissingScripts(obj, assetPath);
                }
            }
            else if (assetPath.EndsWith(".unity"))
            {
                CheckSceneForMissingScripts(assetPath);
            }
        }

        Debug.Log($"Total assets with missing scripts: {missingScriptEntries.Count}");
        Repaint();
    }

    private void CheckSceneForMissingScripts(string scenePath)
    {
        // Open the scene in Single mode without affecting the currently open scenes
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Could not open scene: {scenePath}");
            return;
        }

        // Traverse the scene's root GameObjects
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            CheckForMissingScripts(rootObj, scenePath);
        }

        // Close the scene to avoid clutter
        EditorSceneManager.CloseScene(scene, true);
    }

    private void CheckForMissingScripts(GameObject obj, string assetPath)
    {
        Component[] components = obj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                var entry = new MissingScriptEntry(assetPath, obj.name);

                if (ignoreList.Contains(entry))
                {
                    ignoredItemsFound++;
                }
                else
                {
                    missingScriptEntries.Add(entry);
                }
            }
        }

        foreach (Transform child in obj.transform)
        {
            CheckForMissingScripts(child.gameObject, assetPath);
        }
    }

    private void OpenPrefabAndSelect(string assetPath, string objectName)
    {
        PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(assetPath);
        if (prefabStage != null)
        {
            GameObject prefabRoot = prefabStage.prefabContentsRoot;
            GameObject targetObject = FindGameObjectInPrefab(prefabRoot, objectName);

            if (targetObject != null)
            {
                Selection.activeObject = targetObject;
                EditorGUIUtility.PingObject(targetObject);
            }
        }
        else
        {
            Debug.LogWarning($"Could not open prefab at {assetPath}");
        }
    }

    private void OpenSceneAndSelect(string scenePath, string objectName)
    {
        // Open the scene in Single mode
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Could not open scene: {scenePath}");
            return;
        }

        // Find the GameObject in the scene by name
        GameObject targetObject = FindGameObjectInScene(scene, objectName);

        if (targetObject != null)
        {
            Selection.activeObject = targetObject;
            EditorGUIUtility.PingObject(targetObject);
        }
        else
        {
            Debug.LogWarning($"GameObject '{objectName}' not found in scene: {scenePath}");
        }
    }

    private GameObject FindGameObjectInScene(Scene scene, string objectName)
    {
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            GameObject found = FindGameObjectInHierarchy(rootObj, objectName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    private GameObject FindGameObjectInHierarchy(GameObject root, string objectName)
    {
        if (root.name == objectName)
            return root;

        foreach (Transform child in root.transform)
        {
            GameObject found = FindGameObjectInHierarchy(child.gameObject, objectName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    private GameObject FindGameObjectInPrefab(GameObject root, string objectName)
    {
        if (root.name == objectName)
            return root;

        foreach (Transform child in root.transform)
        {
            GameObject result = FindGameObjectInPrefab(child.gameObject, objectName);
            if (result != null)
                return result;
        }

        return null;
    }

    private void LoadIgnoreList()
    {
        ignoreList.Clear();
        string serializedList = EditorPrefs.GetString(IgnoreListKey, "");
        if (!string.IsNullOrEmpty(serializedList))
        {
            string[] entries = serializedList.Split(';');
            foreach (string entry in entries)
            {
                if (!string.IsNullOrEmpty(entry))
                {
                    string[] parts = entry.Split('|');
                    if (parts.Length == 2)
                    {
                        ignoreList.Add(new MissingScriptEntry(parts[0], parts[1]));
                    }
                }
            }
        }
    }

    private void SaveIgnoreList()
    {
        List<string> serializedEntries = new List<string>();
        foreach (var entry in ignoreList)
        {
            serializedEntries.Add($"{entry.assetPath}|{entry.objectName}");
        }

        string serializedList = string.Join(";", serializedEntries);
        EditorPrefs.SetString(IgnoreListKey, serializedList);
    }
}
