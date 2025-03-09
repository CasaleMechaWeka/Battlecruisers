using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptsFinder : EditorWindow
{
    // Holds individual missing script entries (assetPath and object name)
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
                return assetPath == other.assetPath && objectName == other.objectName;
            return false;
        }

        public override int GetHashCode() => (assetPath + objectName).GetHashCode();
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

    // Only one replacement option at the asset level.
    private MonoScript replacementScript = null;

    // Dictionaries for foldout states (grouping by asset path)
    private Dictionary<string, bool> assetFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, bool> ignoreFoldoutStates = new Dictionary<string, bool>();

    private const string IgnoreListKey = "MissingScriptsIgnoreList";

    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptsFinder>("Missing Scripts Finder");
    }

    private void OnEnable() { LoadIgnoreList(); }

    private void OnDisable()
    {
        if (keepIgnoreList)
            SaveIgnoreList();
    }

    private void OnGUI()
    {
        // Top row: Find Missing Scripts button and toggle between Main/Ignore lists.
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }
        if (GUILayout.Button(viewIgnoreList ? $"View Main List ({missingScriptEntries.Count})" : $"View Ignore List ({ignoreList.Count})", GUILayout.Width(150)))
        {
            viewIgnoreList = !viewIgnoreList;
        }
        EditorGUILayout.EndHorizontal();

        // Replacement Script field and Replace All button on the same line.
        EditorGUILayout.BeginHorizontal();
        replacementScript = (MonoScript)EditorGUILayout.ObjectField("Replacement Script", replacementScript, typeof(MonoScript), false);
        EditorGUI.BeginDisabledGroup(replacementScript == null || missingScriptEntries.Count == 0);
        if (GUILayout.Button("Replace All", GUILayout.Width(100)))
        {
            ReplaceAllMissing(replacementScript);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        // Show either the ignore list or the missing scripts list.
        if (viewIgnoreList)
            DisplayIgnoreList();
        else
            DisplayMissingScriptsList();

        // Bottom UI Layout.
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"Ignored items found: {ignoredItemsFound}", GUILayout.Width(180));
        GUILayout.FlexibleSpace();
        skipTestingScenes = EditorGUILayout.Toggle("Skip Testing Scenes", skipTestingScenes, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        skipTrashScenes = EditorGUILayout.Toggle("Skip Trash Scenes", skipTrashScenes, GUILayout.Width(200));
        GUILayout.FlexibleSpace();
        keepIgnoreList = EditorGUILayout.Toggle("Keep Ignore List", keepIgnoreList, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
    }


    private void DisplayMissingScriptsList()
    {
        if (missingScriptEntries.Count == 0)
        {
            EditorGUILayout.LabelField("No missing scripts found.");
            return;
        }

        EditorGUILayout.LabelField($"Results ({missingScriptEntries.Count})", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Group entries by asset path.
        Dictionary<string, List<string>> groupedEntries = new Dictionary<string, List<string>>();
        foreach (var entry in missingScriptEntries)
        {
            if (!groupedEntries.ContainsKey(entry.assetPath))
                groupedEntries[entry.assetPath] = new List<string>();
            if (!groupedEntries[entry.assetPath].Contains(entry.objectName))
                groupedEntries[entry.assetPath].Add(entry.objectName);
        }

        foreach (var kvp in groupedEntries)
        {
            string assetPath = kvp.Key;
            List<string> missingObjects = kvp.Value;

            if (missingObjects.Count == 1)
            {
                // Single missing reference: show a single row with Jump, Replace and Ignore.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{assetPath} -> {missingObjects[0]}", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                {
                    JumpToReference(assetPath, missingObjects[0]);
                }
                if (replacementScript != null && GUILayout.Button("Replace", GUILayout.Width(60)))
                {
                    ReplaceMissingAssetGroup(assetPath, replacementScript);
                }
                if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                {
                    var entry = new MissingScriptEntry(assetPath, missingObjects[0]);
                    if (!ignoreList.Contains(entry))
                        ignoreList.Add(entry);
                    missingScriptEntries.RemoveAll(e => e.assetPath == assetPath && e.objectName == missingObjects[0]);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                // Multiple missing references: display a foldout for the asset.
                bool expanded = false;
                if (!assetFoldoutStates.TryGetValue(assetPath, out expanded))
                    assetFoldoutStates[assetPath] = false;
                EditorGUILayout.BeginHorizontal();
                assetFoldoutStates[assetPath] = EditorGUILayout.Foldout(assetFoldoutStates[assetPath], $"{assetPath} ({missingObjects.Count})", true);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                {
                    OpenAssetAndSelectRoot(assetPath);
                }
                if (replacementScript != null && GUILayout.Button("Replace", GUILayout.Width(60)))
                {
                    ReplaceMissingAssetGroup(assetPath, replacementScript);
                }
                if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                {
                    foreach (var objName in missingObjects)
                    {
                        var entry = new MissingScriptEntry(assetPath, objName);
                        if (!ignoreList.Contains(entry))
                            ignoreList.Add(entry);
                    }
                    missingScriptEntries.RemoveAll(e => e.assetPath == assetPath);
                }
                EditorGUILayout.EndHorizontal();

                // Expanded list: each individual missing reference gets its own row with Jump and Ignore.
                if (assetFoldoutStates[assetPath])
                {
                    EditorGUI.indentLevel++;
                    foreach (var objName in missingObjects)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(objName, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("Jump", GUILayout.Width(50)))
                        {
                            JumpToReference(assetPath, objName);
                        }
                        if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                        {
                            var entry = new MissingScriptEntry(assetPath, objName);
                            if (!ignoreList.Contains(entry))
                                ignoreList.Add(entry);
                            // Remove only this missing reference from the list.
                            missingScriptEntries.RemoveAll(e => e.assetPath == assetPath && e.objectName == objName);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }


    private void DisplayIgnoreList()
    {
        EditorGUILayout.LabelField($"Ignore List ({ignoreList.Count})", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear All", GUILayout.Width(80)))
        {
            if (EditorUtility.DisplayDialog("Clear Ignore List", "Are you sure you want to clear the ignore list?", "Yes", "No"))
                ignoreList.Clear();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Dictionary<string, List<string>> groupedIgnored = new Dictionary<string, List<string>>();
        foreach (var entry in ignoreList)
        {
            if (!groupedIgnored.ContainsKey(entry.assetPath))
                groupedIgnored[entry.assetPath] = new List<string>();
            if (!groupedIgnored[entry.assetPath].Contains(entry.objectName))
                groupedIgnored[entry.assetPath].Add(entry.objectName);
        }

        foreach (var kvp in groupedIgnored)
        {
            string assetPath = kvp.Key;
            List<string> ignoredObjects = kvp.Value;

            if (ignoredObjects.Count == 1)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{assetPath} -> {ignoredObjects[0]}", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    JumpToReference(assetPath, ignoredObjects[0]);
                if (GUILayout.Button("Remove", GUILayout.Width(50)))
                {
                    var entry = new MissingScriptEntry(assetPath, ignoredObjects[0]);
                    ignoreList.RemoveAll(e => e.assetPath == assetPath && e.objectName == ignoredObjects[0]);
                    missingScriptEntries.Add(entry);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                bool expanded = false;
                if (!ignoreFoldoutStates.TryGetValue(assetPath, out expanded))
                    ignoreFoldoutStates[assetPath] = false;
                EditorGUILayout.BeginHorizontal();
                ignoreFoldoutStates[assetPath] = EditorGUILayout.Foldout(ignoreFoldoutStates[assetPath], $"{assetPath} ({ignoredObjects.Count})", true);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    OpenAssetAndSelectRoot(assetPath);
                if (GUILayout.Button("Remove All", GUILayout.Width(50)))
                {
                    foreach (var objName in ignoredObjects)
                    {
                        var entry = new MissingScriptEntry(assetPath, objName);
                        missingScriptEntries.Add(entry);
                    }
                    ignoreList.RemoveAll(e => e.assetPath == assetPath);
                }
                EditorGUILayout.EndHorizontal();

                if (ignoreFoldoutStates[assetPath])
                {
                    EditorGUI.indentLevel++;
                    foreach (var objName in ignoredObjects)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(objName, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("Jump", GUILayout.Width(50)))
                            JumpToReference(assetPath, objName);
                        if (GUILayout.Button("Remove", GUILayout.Width(50)))
                        {
                            var entry = new MissingScriptEntry(assetPath, objName);
                            ignoreList.RemoveAll(e => e.assetPath == assetPath && e.objectName == objName);
                            missingScriptEntries.Add(entry);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void FindMissingScripts()
    {
        missingScriptEntries.Clear();
        ignoredItemsFound = 0;
        assetFoldoutStates.Clear();
        ignoreFoldoutStates.Clear();

        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssets)
        {
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
                    CheckForMissingScripts(obj, assetPath);
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
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Could not open scene: {scenePath}");
            return;
        }
        foreach (GameObject rootObj in scene.GetRootGameObjects())
            CheckForMissingScripts(rootObj, scenePath);
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
                    ignoredItemsFound++;
                else
                    missingScriptEntries.Add(entry);
            }
        }
        foreach (Transform child in obj.transform)
            CheckForMissingScripts(child.gameObject, assetPath);
    }

    private void JumpToReference(string assetPath, string objectName)
    {
        if (assetPath.EndsWith(".prefab"))
            OpenPrefabAndSelect(assetPath, objectName);
        else if (assetPath.EndsWith(".unity"))
            OpenSceneAndSelect(assetPath, objectName);
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
            Debug.LogWarning($"Could not open prefab at {assetPath}");
    }

    private void OpenSceneAndSelect(string scenePath, string objectName)
    {
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Could not open scene: {scenePath}");
            return;
        }
        GameObject targetObject = FindGameObjectInScene(scene, objectName);
        if (targetObject != null)
        {
            Selection.activeObject = targetObject;
            EditorGUIUtility.PingObject(targetObject);
        }
        else
            Debug.LogWarning($"GameObject '{objectName}' not found in scene: {scenePath}");
    }

    private void OpenAssetAndSelectRoot(string assetPath)
    {
        if (assetPath.EndsWith(".prefab"))
        {
            PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(assetPath);
            if (prefabStage != null)
            {
                GameObject prefabRoot = prefabStage.prefabContentsRoot;
                Selection.activeObject = prefabRoot;
                EditorGUIUtility.PingObject(prefabRoot);
            }
            else
                Debug.LogWarning($"Could not open prefab at {assetPath}");
        }
        else if (assetPath.EndsWith(".unity"))
        {
            Scene scene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Single);
            if (scene.isLoaded)
            {
                GameObject[] roots = scene.GetRootGameObjects();
                if (roots.Length > 0)
                {
                    Selection.activeObject = roots[0];
                    EditorGUIUtility.PingObject(roots[0]);
                }
                else
                    Debug.LogWarning($"No root GameObjects found in scene: {assetPath}");
            }
            else
                Debug.LogWarning($"Could not open scene: {assetPath}");
        }
    }

    private GameObject FindGameObjectInScene(Scene scene, string objectName)
    {
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            GameObject found = FindGameObjectInHierarchy(rootObj, objectName);
            if (found != null)
                return found;
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
                return found;
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

    //–––––– Replacement Functionality ––––––

    // Processes an entire asset (prefab or scene) identified by its asset path.
    // It loads the asset, iterates over every GameObject in its hierarchy, removes ALL missing references,
    // and if any were removed from a GameObject, it adds the replacement component.
    // Finally, it saves the asset.
    private void ReplaceMissingAssetGroup(string assetPath, MonoScript replacementScript)
    {
        if (replacementScript == null)
            return;
        Type newType = replacementScript.GetClass();
        if (newType == null)
        {
            Debug.LogWarning("Replacement script does not have a valid class.");
            return;
        }

        if (assetPath.EndsWith(".prefab"))
        {
            GameObject prefabContents = PrefabUtility.LoadPrefabContents(assetPath);
            ProcessAssetForMissingReferences(prefabContents, newType);
            PrefabUtility.SaveAsPrefabAsset(prefabContents, assetPath);
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
        else if (assetPath.EndsWith(".unity"))
        {
            Scene scene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Single);
            GameObject[] roots = scene.GetRootGameObjects();
            foreach (GameObject root in roots)
                ProcessAssetForMissingReferences(root, newType);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
        // Remove this asset group from our missing list after processing.
        missingScriptEntries.RemoveAll(e => e.assetPath == assetPath);
    }

    // Global "Replace All" that processes every asset group.
    private void ReplaceAllMissing(MonoScript replacementScript)
    {
        Dictionary<string, List<string>> groupedEntries = new Dictionary<string, List<string>>();
        foreach (var entry in missingScriptEntries)
        {
            if (!groupedEntries.ContainsKey(entry.assetPath))
                groupedEntries[entry.assetPath] = new List<string>();
            if (!groupedEntries[entry.assetPath].Contains(entry.objectName))
                groupedEntries[entry.assetPath].Add(entry.objectName);
        }
        foreach (var kvp in groupedEntries)
            ReplaceMissingAssetGroup(kvp.Key, replacementScript);
    }

    // Iterates over every GameObject (including inactive ones) in the asset hierarchy.
    // For each GameObject, repeatedly calls RemoveMonoBehavioursWithMissingScript (with a cap on iterations)
    // and if any missing reference was removed, adds the new component.
    private void ProcessAssetForMissingReferences(GameObject root, Type newType)
    {
        Transform[] allTransforms = root.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            GameObject go = t.gameObject;
            int removed = RemoveMissingReferencesFromGameObject(go);
            if (removed > 0)
                go.AddComponent(newType);
        }
    }

    // Removes all missing references from a single GameObject.
    // Uses a capped loop to ensure it doesn't get stuck.
    private int RemoveMissingReferencesFromGameObject(GameObject go)
    {
        int totalRemoved = 0;
        int iteration = 0;
        int maxIteration = 10; // safeguard to avoid infinite loops
        while (iteration < maxIteration)
        {
            int removedThisRound = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (removedThisRound == 0)
                break;
            totalRemoved += removedThisRound;
            iteration++;
        }
        return totalRemoved;
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
                        ignoreList.Add(new MissingScriptEntry(parts[0], parts[1]));
                }
            }
        }
    }

    private void SaveIgnoreList()
    {
        List<string> serializedEntries = new List<string>();
        foreach (var entry in ignoreList)
            serializedEntries.Add($"{entry.assetPath}|{entry.objectName}");
        string serializedList = string.Join(";", serializedEntries);
        EditorPrefs.SetString(IgnoreListKey, serializedList);
    }
}
