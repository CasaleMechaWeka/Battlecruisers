using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptsFinder : EditorWindow
{
    // Updated struct to store a unique path for each missing reference.
    private struct MissingScriptEntry
    {
        public string assetPath;
        public string objectName;    // the GameObject's name
        public string uniquePath;    // full unique hierarchy path

        public MissingScriptEntry(string assetPath, string objectName, string uniquePath)
        {
            this.assetPath = assetPath;
            this.objectName = objectName;
            this.uniquePath = uniquePath;
        }

        public override bool Equals(object obj)
        {
            if (obj is MissingScriptEntry other)
            {
                // Use asset path and uniquePath for equality.
                return assetPath == other.assetPath && uniquePath == other.uniquePath;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (assetPath + uniquePath).GetHashCode();
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

    // Replacement option (asset-level only).
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

    #region GUI

    private void OnGUI()
    {
        // Top row: "Find Missing Scripts", "Refresh", and the toggle between Main/Ignore lists.
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }
        if (GUILayout.Button("Refresh", GUILayout.Width(80)))
        {
            RefreshMissingReferences();
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

    // Helper: Recursively build a unique path for a GameObject.
    // It appends a sibling index among those with the same name.
    private string GetUniquePath(GameObject go)
    {
        if (go.transform.parent == null)
            return go.name;
        string parentPath = GetUniquePath(go.transform.parent.gameObject);
        // Count siblings with the same name preceding this object.
        int index = 0;
        foreach (Transform sibling in go.transform.parent)
        {
            if (sibling.gameObject.name == go.name)
            {
                if (sibling.gameObject == go)
                    break;
                index++;
            }
        }
        return parentPath + "/" + go.name + "[" + index + "]";
    }

    // Update CheckForMissingScripts to store the unique path.
    private void CheckForMissingScripts(GameObject obj, string assetPath)
    {
        Component[] components = obj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                string uniquePath = GetUniquePath(obj);
                var entry = new MissingScriptEntry(assetPath, obj.name, uniquePath);
                // Avoid duplicate entries (using uniquePath now).
                if (!missingScriptEntries.Contains(entry))
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

    // Display functions now group by assetPath and store full unique entries.
    private void DisplayMissingScriptsList()
    {
        // Filter out any entries that are in the ignore list.
        List<MissingScriptEntry> filteredEntries = missingScriptEntries.FindAll(entry => !ignoreList.Contains(entry));

        if (filteredEntries.Count == 0)
        {
            EditorGUILayout.LabelField("No missing scripts found.");
            return;
        }

        EditorGUILayout.LabelField($"Results ({filteredEntries.Count})", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Group filtered entries by asset path.
        Dictionary<string, List<MissingScriptEntry>> groupedEntries = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (var entry in filteredEntries)
        {
            if (!groupedEntries.ContainsKey(entry.assetPath))
                groupedEntries[entry.assetPath] = new List<MissingScriptEntry>();
            groupedEntries[entry.assetPath].Add(entry);
        }

        foreach (var kvp in groupedEntries)
        {
            string assetPath = kvp.Key;
            List<MissingScriptEntry> entries = kvp.Value;

            if (entries.Count == 1)
            {
                // Single missing reference: show a row with Jump, Replace and Ignore.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{assetPath} -> {entries[0].uniquePath}", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    JumpToReference(assetPath, entries[0].uniquePath);
                if (replacementScript != null && GUILayout.Button("Replace", GUILayout.Width(60)))
                    ReplaceMissingAssetGroup(assetPath, replacementScript);
                if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                {
                    if (!ignoreList.Contains(entries[0]))
                        ignoreList.Add(entries[0]);
                    missingScriptEntries.RemoveAll(e => e.assetPath == assetPath && e.uniquePath == entries[0].uniquePath);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                // Multiple missing references: show foldout.
                bool expanded = false;
                if (!assetFoldoutStates.TryGetValue(assetPath, out expanded))
                    assetFoldoutStates[assetPath] = false;
                EditorGUILayout.BeginHorizontal();
                assetFoldoutStates[assetPath] = EditorGUILayout.Foldout(assetFoldoutStates[assetPath], $"{assetPath} ({entries.Count})", true);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    OpenAssetAndSelectRoot(assetPath);
                if (replacementScript != null && GUILayout.Button("Replace", GUILayout.Width(60)))
                    ReplaceMissingAssetGroup(assetPath, replacementScript);
                if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                {
                    foreach (var entry in entries)
                    {
                        if (!ignoreList.Contains(entry))
                            ignoreList.Add(entry);
                    }
                    missingScriptEntries.RemoveAll(e => e.assetPath == assetPath);
                }
                EditorGUILayout.EndHorizontal();

                // Expanded list: display each individual missing reference.
                if (assetFoldoutStates[assetPath])
                {
                    EditorGUI.indentLevel++;
                    foreach (var entry in entries)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(entry.uniquePath, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("Jump", GUILayout.Width(50)))
                            JumpToReference(assetPath, entry.uniquePath);
                        if (GUILayout.Button("Ignore", GUILayout.Width(50)))
                        {
                            if (!ignoreList.Contains(entry))
                                ignoreList.Add(entry);
                            missingScriptEntries.RemoveAll(e => e.assetPath == assetPath && e.uniquePath == entry.uniquePath);
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
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Ignore List ({ignoreList.Count})", EditorStyles.boldLabel);
        if (GUILayout.Button("Clear All", GUILayout.Width(80)))
        {
            if (EditorUtility.DisplayDialog("Clear Ignore List", "Are you sure you want to clear the ignore list?", "Yes", "No"))
                ignoreList.Clear();
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Dictionary<string, List<MissingScriptEntry>> groupedIgnored = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (var entry in ignoreList)
        {
            if (!groupedIgnored.ContainsKey(entry.assetPath))
                groupedIgnored[entry.assetPath] = new List<MissingScriptEntry>();
            groupedIgnored[entry.assetPath].Add(entry);
        }

        foreach (var kvp in groupedIgnored)
        {
            string assetPath = kvp.Key;
            List<MissingScriptEntry> entries = kvp.Value;

            if (entries.Count == 1)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{assetPath} -> {entries[0].uniquePath}", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    JumpToReference(assetPath, entries[0].uniquePath);
                if (GUILayout.Button("Remove", GUILayout.Width(50)))
                {
                    ignoreList.RemoveAll(e => e.assetPath == assetPath && e.uniquePath == entries[0].uniquePath);
                    missingScriptEntries.Add(entries[0]);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                bool expanded = false;
                if (!ignoreFoldoutStates.TryGetValue(assetPath, out expanded))
                    ignoreFoldoutStates[assetPath] = false;
                EditorGUILayout.BeginHorizontal();
                ignoreFoldoutStates[assetPath] = EditorGUILayout.Foldout(ignoreFoldoutStates[assetPath], $"{assetPath} ({entries.Count})", true);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    OpenAssetAndSelectRoot(assetPath);
                if (GUILayout.Button("Remove All", GUILayout.Width(50)))
                {
                    foreach (var entry in entries)
                        missingScriptEntries.Add(entry);
                    ignoreList.RemoveAll(e => e.assetPath == assetPath);
                }
                EditorGUILayout.EndHorizontal();

                if (ignoreFoldoutStates[assetPath])
                {
                    EditorGUI.indentLevel++;
                    foreach (var entry in entries)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(entry.uniquePath, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("Jump", GUILayout.Width(50)))
                            JumpToReference(assetPath, entry.uniquePath);
                        if (GUILayout.Button("Remove", GUILayout.Width(50)))
                        {
                            ignoreList.RemoveAll(e => e.assetPath == assetPath && e.uniquePath == entry.uniquePath);
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
        // Clear old data
        missingScriptEntries.Clear();
        ignoredItemsFound = 0;
        assetFoldoutStates.Clear();
        ignoreFoldoutStates.Clear();

        // Get all asset paths.
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        int totalAssets = allAssets.Length;
        int processedAssets = 0;
        int missingFound = 0;

        // Iterate through assets.
        foreach (string assetPath in allAssets)
        {
            processedAssets++;

            // Skip some asset paths.
            if (assetPath.StartsWith("Packages/"))
                continue;
            if (skipTestingScenes && assetPath.StartsWith("Assets/Scenes/Testing/"))
                continue;
            if (skipTrashScenes && assetPath.StartsWith("Assets/Scenes/Trash/"))
                continue;

            // Process prefabs.
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (obj != null)
                {
                    int countBefore = missingScriptEntries.Count;
                    CheckForMissingScripts(obj, assetPath);
                    missingFound += missingScriptEntries.Count - countBefore;
                }
            }
            // Process scenes.
            else if (assetPath.EndsWith(".unity"))
            {
                CheckSceneForMissingScripts(assetPath);
                // Note: You can also update missingFound by checking the change in missingScriptEntries.Count.
            }

            // Update the progress bar every asset.
            float progress = (float)processedAssets / totalAssets;
            EditorUtility.DisplayProgressBar(
                "Finding Missing Scripts",
                $"Processing asset {processedAssets}/{totalAssets} Missing References Found: {missingFound}",
                progress);
        }

        // Clear the progress bar when done.
        EditorUtility.ClearProgressBar();

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

    // Add this helper to check if a GameObject still has missing scripts.
    private bool HasMissingScripts(GameObject go)
    {
        Component[] comps = go.GetComponents<Component>();
        foreach (Component comp in comps)
        {
            if (comp == null)
                return true;
        }
        return false;
    }

    // Refreshes the missingScriptEntries list by checking each entry's object in its asset.
    private void RefreshMissingReferences()
    {
        // Group entries by assetPath.
        Dictionary<string, List<MissingScriptEntry>> groups = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (var entry in missingScriptEntries)
        {
            if (!groups.ContainsKey(entry.assetPath))
                groups[entry.assetPath] = new List<MissingScriptEntry>();
            groups[entry.assetPath].Add(entry);
        }

        List<MissingScriptEntry> toRemove = new List<MissingScriptEntry>();

        foreach (var kvp in groups)
        {
            string assetPath = kvp.Key;
            List<MissingScriptEntry> entries = kvp.Value;
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
                if (prefabRoot != null)
                {
                    foreach (var entry in entries)
                    {
                        GameObject obj = FindGameObjectByUniquePath(prefabRoot, entry.uniquePath);
                        // If found and the object no longer has any missing scripts, mark for removal.
                        if (obj != null && !HasMissingScripts(obj))
                        {
                            toRemove.Add(entry);
                        }
                    }
                    PrefabUtility.UnloadPrefabContents(prefabRoot);
                }
            }
            else if (assetPath.EndsWith(".unity"))
            {
                Scene scene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);
                if (scene.isLoaded)
                {
                    foreach (var entry in entries)
                    {
                        GameObject target = null;
                        foreach (GameObject rootObj in scene.GetRootGameObjects())
                        {
                            target = FindGameObjectByUniquePath(rootObj, entry.uniquePath);
                            if (target != null)
                                break;
                        }
                        if (target != null && !HasMissingScripts(target))
                        {
                            toRemove.Add(entry);
                        }
                    }
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
        // Remove entries that are now fixed.
        foreach (var entry in toRemove)
        {
            missingScriptEntries.RemoveAll(e => e.assetPath == entry.assetPath && e.uniquePath == entry.uniquePath);
        }
        Repaint();
    }

    #endregion
    #region Jump/Select

    //–––––– Jump/Select Functions ––––––

    // Uses the unique path to locate the correct GameObject.
    private GameObject FindGameObjectByUniquePath(GameObject root, string uniquePath)
    {
        // Split the path by '/'
        string[] parts = uniquePath.Split('/');
        if (parts.Length == 0)
            return null;

        // The root's name should match the first part (ignoring any sibling index)
        string rootName = parts[0];
        int bracket = rootName.IndexOf('[');
        if (bracket >= 0)
            rootName = rootName.Substring(0, bracket);
        if (root.name != rootName)
            return null;

        GameObject current = root;
        for (int i = 1; i < parts.Length; i++)
        {
            string part = parts[i];
            string childName = part;
            int siblingIndex = 0;
            int bIndex = part.IndexOf('[');
            if (bIndex >= 0)
            {
                childName = part.Substring(0, bIndex);
                int endBracket = part.IndexOf(']', bIndex);
                if (endBracket > bIndex)
                {
                    int.TryParse(part.Substring(bIndex + 1, endBracket - bIndex - 1), out siblingIndex);
                }
            }
            // Look for all children with matching name.
            List<Transform> matches = new List<Transform>();
            foreach (Transform child in current.transform)
            {
                if (child.gameObject.name == childName)
                    matches.Add(child);
            }
            if (matches.Count <= siblingIndex)
                return null;
            current = matches[siblingIndex].gameObject;
        }
        return current;
    }

    private void JumpToReference(string assetPath, string uniquePath)
    {
        if (assetPath.EndsWith(".prefab"))
        {
            OpenPrefabAndSelect(assetPath, uniquePath);
        }
        else if (assetPath.EndsWith(".unity"))
        {
            OpenSceneAndSelect(assetPath, uniquePath);
        }
    }

    // Updated jump for prefabs: first check for an open (possibly unsaved) prefab stage.
    private void OpenPrefabAndSelect(string assetPath, string uniquePath)
    {
        // Check if a prefab stage is already open.
        PrefabStage currentStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (currentStage != null)
        {
            // If the current stage is for the same asset (or assetPath is empty, meaning unsaved)
            if (string.IsNullOrEmpty(assetPath) || currentStage.assetPath == assetPath)
            {
                GameObject prefabRoot = currentStage.prefabContentsRoot;
                if (prefabRoot != null)
                {
                    GameObject targetObject = FindGameObjectByUniquePath(prefabRoot, uniquePath);
                    if (targetObject != null)
                    {
                        Selection.activeObject = targetObject;
                        EditorGUIUtility.PingObject(targetObject);
                        return;
                    }
                }
            }
        }
        // Fallback: open the prefab normally.
        PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(assetPath);
        if (prefabStage != null)
        {
            GameObject prefabRoot = prefabStage.prefabContentsRoot;
            GameObject targetObject = FindGameObjectByUniquePath(prefabRoot, uniquePath);
            if (targetObject != null)
            {
                Selection.activeObject = targetObject;
                EditorGUIUtility.PingObject(targetObject);
            }
            else
            {
                Debug.LogWarning($"Could not find object with path {uniquePath} in prefab: {assetPath}");
            }
        }
        else
        {
            Debug.LogWarning($"Could not open prefab at {assetPath}");
        }
    }

    // Updated jump for scenes: if the current active scene is unsaved (or matches the asset path), use it.
    private void OpenSceneAndSelect(string scenePath, string uniquePath)
    {
        Scene currentScene = EditorSceneManager.GetActiveScene();
        if ((string.IsNullOrEmpty(scenePath) || currentScene.path == scenePath) && currentScene.isLoaded)
        {
            GameObject targetObject = null;
            foreach (GameObject rootObj in currentScene.GetRootGameObjects())
            {
                targetObject = FindGameObjectByUniquePath(rootObj, uniquePath);
                if (targetObject != null)
                    break;
            }
            if (targetObject != null)
            {
                Selection.activeObject = targetObject;
                EditorGUIUtility.PingObject(targetObject);
                return;
            }
        }
        // Fallback: open the scene normally.
        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Could not open scene: {scenePath}");
            return;
        }
        GameObject found = null;
        foreach (GameObject rootObj in scene.GetRootGameObjects())
        {
            found = FindGameObjectByUniquePath(rootObj, uniquePath);
            if (found != null)
                break;
        }
        if (found != null)
        {
            Selection.activeObject = found;
            EditorGUIUtility.PingObject(found);
        }
        else
        {
            Debug.LogWarning($"Could not find object with path {uniquePath} in scene: {scenePath}");
        }
    }

    private void OpenAssetAndSelectRoot(string assetPath)
    {
        if (assetPath.EndsWith(".prefab"))
        {
            // Open the prefab in its dedicated prefab stage.
            PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(assetPath);
            if (prefabStage != null)
            {
                GameObject prefabRoot = prefabStage.prefabContentsRoot;
                if (prefabRoot != null)
                {
                    Selection.activeObject = prefabRoot;
                    EditorGUIUtility.PingObject(prefabRoot);
                }
                else
                {
                    Debug.LogWarning($"Could not get prefab root for: {assetPath}");
                }
            }
            else
            {
                Debug.LogWarning($"Could not open prefab at {assetPath}");
            }
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
                {
                    Debug.LogWarning($"No root GameObjects found in scene: {assetPath}");
                }
            }
            else
            {
                Debug.LogWarning($"Could not open scene: {assetPath}");
            }
        }
    }

    #endregion
    #region Replace

    //–––––– Replacement Functionality – Asset-Level Only ––––––

    // Processes an entire asset (prefab or scene) by asset path.
    // It loads the asset, iterates over every GameObject in its hierarchy (including inactive ones),
    // removes ALL missing references, and if any were removed on an object, adds the replacement component.
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
        Dictionary<string, List<MissingScriptEntry>> groupedEntries = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (var entry in missingScriptEntries)
        {
            if (!groupedEntries.ContainsKey(entry.assetPath))
                groupedEntries[entry.assetPath] = new List<MissingScriptEntry>();
            groupedEntries[entry.assetPath].Add(entry);
        }
        foreach (var kvp in groupedEntries)
        {
            ReplaceMissingAssetGroup(kvp.Key, replacementScript);
        }
    }

    #endregion

    // Iterates over every GameObject (including inactive ones) in the asset hierarchy.
    // For each GameObject, repeatedly calls RemoveMonoBehavioursWithMissingScript (with a cap) and if any were removed, adds the replacement component.
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

    // Removes all missing references from a single GameObject using a capped loop.
    private int RemoveMissingReferencesFromGameObject(GameObject go)
    {
        int totalRemoved = 0;
        int iteration = 0;
        int maxIteration = 10; // safeguard
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
                    if (parts.Length == 3)
                    {
                        // Stored format: assetPath|objectName|uniquePath
                        ignoreList.Add(new MissingScriptEntry(parts[0], parts[1], parts[2]));
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
            serializedEntries.Add($"{entry.assetPath}|{entry.objectName}|{entry.uniquePath}");
        }
        string serializedList = string.Join(";", serializedEntries);
        EditorPrefs.SetString(IgnoreListKey, serializedList);
    }
}
