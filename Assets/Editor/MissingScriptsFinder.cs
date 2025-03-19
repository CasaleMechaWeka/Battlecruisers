using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptsFinder : EditorWindow
{
    #region Data Structures
    // Stores a unique path for each missing reference.
    private struct MissingScriptEntry
    {
        public string assetPath;
        public string objectName;    // The GameObject's name.
        public string uniquePath;    // Full unique hierarchy path.

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
    #endregion

    #region Fields
    private List<MissingScriptEntry> missingScriptEntries = new List<MissingScriptEntry>();
    private List<MissingScriptEntry> ignoreList = new List<MissingScriptEntry>();
    private int ignoredItemsFound = 0;
    private Vector2 scrollPosition;
    private bool viewIgnoreList = false;
    private bool keepIgnoreList = true;
    private bool onlyScanBuildScenes = false; // Default: scan all scenes
    private bool skipTestingScenes = true;
    private bool skipTrashScenes = true;
    private bool skipTestPrefabs = true;

    private bool filterScenes = true;
    private bool filterPrefabs = true;
    private bool filterMissingScripts = true;
    private bool filterMissingReferences = true;

    private bool showSceneDropdown = false; // Controls dropdown visibility
    private Dictionary<string, bool> sceneSelection = new Dictionary<string, bool>(); // Stores scene states
    private Vector2 sceneDropdownScroll = Vector2.zero; // Scroll position for dropdown


    // Replacement option (asset-level only).
    private MonoScript replacementScript = null;

    // Dictionaries for foldout states (grouping by asset path).
    private Dictionary<string, bool> assetFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, bool> ignoreFoldoutStates = new Dictionary<string, bool>();

    private const string IgnoreListKey = "MissingScriptsIgnoreList";
    #endregion

    #region GUI
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptsFinder>("Missing Scripts Finder");
    }

    private void OnEnable()
    {
        LoadIgnoreList();
        LoadCacheFromDisk();
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
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }
        if (GUILayout.Button("Clear Cache", GUILayout.Width(100)))
        {
            ClearCache();
        }
        if (GUILayout.Button(viewIgnoreList ? $"View Main List ({missingScriptEntries.Count})" : $"View Ignore List ({ignoreList.Count})", GUILayout.Width(150)))
        {
            viewIgnoreList = !viewIgnoreList;
        }
        EditorGUILayout.EndHorizontal();

        // Toggle for scanning only build settings scenes
        onlyScanBuildScenes = EditorGUILayout.Toggle("Only Scan Scenes in Build Settings", onlyScanBuildScenes);

        // 3rd Line: Show Affected Scene/Prefab Count with Icons
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(EditorGUIUtility.IconContent("SceneAsset Icon"), GUILayout.Width(18), GUILayout.Height(18));
        GUILayout.Label($"Scenes: {missingScriptEntries.Count(e => e.assetPath.EndsWith(".unity"))}", GUILayout.Width(80));

        GUILayout.Label(EditorGUIUtility.IconContent("Prefab Icon"), GUILayout.Width(18), GUILayout.Height(18));
        GUILayout.Label($"Prefabs: {missingScriptEntries.Count(e => e.assetPath.EndsWith(".prefab"))}", GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        // 4th Line: Filters for Scenes, Prefabs, Missing Components, and Missing References
        EditorGUILayout.BeginHorizontal();
        filterScenes = EditorGUILayout.ToggleLeft("Scenes", filterScenes, GUILayout.Width(80));
        filterPrefabs = EditorGUILayout.ToggleLeft("Prefabs", filterPrefabs, GUILayout.Width(80));
        filterMissingScripts = EditorGUILayout.ToggleLeft("Missing Scripts", filterMissingScripts, GUILayout.Width(120));
        filterMissingReferences = EditorGUILayout.ToggleLeft("Missing References", filterMissingReferences, GUILayout.Width(160));
        EditorGUILayout.EndHorizontal();

        // Display either Ignore List or Main List
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (viewIgnoreList)
            DisplayIgnoreList();
        else
            DisplayEntryList(missingScriptEntries, false);
        EditorGUILayout.EndScrollView();
    }

    // Helper: Recursively build a unique path for a GameObject.
    // It appends a sibling index among those with the same name.
    private string GetUniquePath(GameObject go)
    {
        if (go.transform.parent == null)
            return go.name; // Root object, no need for a path

        string parentPath = GetUniquePath(go.transform.parent.gameObject);
        Transform parent = go.transform.parent;

        // Count ALL siblings with the same name
        int totalSiblingsWithSameName = 0;
        foreach (Transform sibling in parent)
        {
            if (sibling.gameObject.name == go.name)
                totalSiblingsWithSameName++;
        }

        // If only 1 object with this name exists, no index needed
        if (totalSiblingsWithSameName == 1)
            return $"{parentPath}/{go.name}";

        // Find this object's index among siblings with the same name
        int index = 0;
        foreach (Transform sibling in parent)
        {
            if (sibling == go)
                break;
            if (sibling.gameObject.name == go.name)
                index++;
        }

        return $"{parentPath}/{go.name}[{index}]";
    }

    // Common helper method to draw a grouped list of MissingScriptEntry items.
    // The parameter isIgnore indicates whether we're drawing the ignore list or the main missing list.
    private void DisplayEntryList(List<MissingScriptEntry> list, bool isIgnore)
    {
        if (list.Count == 0)
        {
            EditorGUILayout.LabelField(isIgnore ? "No ignored references found." : "No missing scripts found.");
            return;
        }

        Dictionary<string, List<MissingScriptEntry>> grouped = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (var entry in list)
        {
            if (!grouped.ContainsKey(entry.assetPath))
                grouped[entry.assetPath] = new List<MissingScriptEntry>();
            grouped[entry.assetPath].Add(entry);
        }

        foreach (var kvp in grouped)
        {
            string assetPath = kvp.Key;
            List<MissingScriptEntry> entries = kvp.Value;

            // Apply filters
            bool isScene = assetPath.EndsWith(".unity");
            bool isPrefab = assetPath.EndsWith(".prefab");

            if ((isScene && !filterScenes) || (isPrefab && !filterPrefabs))
                continue;

            bool containsMissingScript = entries.Any(e => e.objectName == "Missing Script");
            bool containsMissingReference = entries.Any(e => e.objectName != "Missing Script");

            if ((!filterMissingScripts && containsMissingScript) || (!filterMissingReferences && containsMissingReference))
                continue;

            // Get asset name without file extension
            string displayName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            GUIContent icon = isPrefab ? EditorGUIUtility.IconContent("Prefab Icon") :
                              isScene ? EditorGUIUtility.IconContent("SceneAsset Icon") :
                              null;

            // If only 1 missing reference, display it directly (no dropdown)
            if (entries.Count == 1)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));

                if (icon != null)
                    GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));

                EditorGUILayout.LabelField($"{displayName} → {entries[0].uniquePath}", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("Jump", GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    JumpToReference(assetPath, entries[0].uniquePath);

                string btnText = isIgnore ? "Remove" : "Ignore";
                if (GUILayout.Button(btnText, GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    if (isIgnore)
                    {
                        ignoreList.Remove(entries[0]);
                        missingScriptEntries.Add(entries[0]);
                    }
                    else
                    {
                        if (!ignoreList.Contains(entries[0]))
                            ignoreList.Add(entries[0]);
                        missingScriptEntries.Remove(entries[0]);
                    }
                }

                EditorGUILayout.EndHorizontal();
                continue;
            }

            // Multiple missing references → Use dropdown
            EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (icon != null)
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));

            bool expanded = false;
            if (!assetFoldoutStates.TryGetValue(assetPath, out expanded))
                assetFoldoutStates[assetPath] = false;
            assetFoldoutStates[assetPath] = EditorGUILayout.Foldout(assetFoldoutStates[assetPath], $"{displayName} ({entries.Count})", true);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Jump", GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                OpenAssetAndSelectRoot(assetPath);

            string groupBtnText = isIgnore ? "Remove All" : "Ignore";
            if (GUILayout.Button(groupBtnText, GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (isIgnore)
                {
                    foreach (var entry in entries)
                        missingScriptEntries.Add(entry);
                    ignoreList.RemoveAll(e => e.assetPath == assetPath);
                }
                else
                {
                    foreach (var entry in entries)
                    {
                        if (!ignoreList.Contains(entry))
                            ignoreList.Add(entry);
                    }
                    missingScriptEntries.RemoveAll(e => e.assetPath == assetPath);
                }
            }

            EditorGUILayout.EndHorizontal();

            if (assetFoldoutStates[assetPath])
            {
                EditorGUI.indentLevel++;
                foreach (var entry in entries)
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.LabelField(entry.uniquePath, GUILayout.ExpandWidth(true));

                    if (GUILayout.Button("Jump", GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                        JumpToReference(assetPath, entry.uniquePath);

                    if (GUILayout.Button(isIgnore ? "Remove" : "Ignore", GUILayout.Width(50), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        if (isIgnore)
                        {
                            ignoreList.Remove(entry);
                            missingScriptEntries.Add(entry);
                        }
                        else
                        {
                            if (!ignoreList.Contains(entry))
                                ignoreList.Add(entry);
                            missingScriptEntries.Remove(entry);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    // Main missing list wrapped in a vertical scroll view.
    private void DisplayMissingScriptsList()
    {
        List<MissingScriptEntry> filtered = missingScriptEntries.FindAll(entry => !ignoreList.Contains(entry));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayEntryList(filtered, false);
        EditorGUILayout.EndScrollView();
    }

    // Ignore list wrapped in a vertical scroll view with a header.
    private void DisplayIgnoreList()
    {
        // Only show the title ONCE, in the header
        if (GUILayout.Button("Clear All", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        {
            if (EditorUtility.DisplayDialog("Clear Ignore List", "Are you sure you want to clear the ignore list?", "Yes", "No"))
                ignoreList.Clear();
        }

        // Scrollable Ignore List
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayEntryList(ignoreList, true); // This function also printed the title before (now fixed)
        EditorGUILayout.EndScrollView();
    }


    #endregion

    #region Caching and Unified Scanning

    // Cache entry type.
    private class CacheEntry
    {
        public DateTime lastWriteTime;
        public List<MissingScriptEntry> entries;
    }

    // Dictionary mapping asset path to its cached scan result.
    private Dictionary<string, CacheEntry> assetCache = new Dictionary<string, CacheEntry>();

    // Collects missing references from a given GameObject (and its children) into the provided output list.
    // This method performs both checks: missing components and missing serialized references.
    private void CollectMissingReferences(GameObject obj, string assetPath, List<MissingScriptEntry> output)
    {
        Component[] comps = obj.GetComponents<Component>();
        foreach (var comp in comps)
        {
            if (comp == null)
            {
                // Missing script component.
                string uniquePath = GetUniquePath(obj);
                var entry = new MissingScriptEntry(assetPath, obj.name, uniquePath);
                if (!output.Contains(entry))
                    output.Add(entry);
            }
            else
            {
                // Check the component's serialized properties for missing references.
                SerializedObject so = new SerializedObject(comp);
                SerializedProperty prop = so.GetIterator();
                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (prop.objectReferenceValue == null && prop.objectReferenceInstanceIDValue != 0)
                        {
                            // Build a display name that indicates the component type and property.
                            string displayName = comp.gameObject.name + " (" + comp.GetType().Name + ":" + prop.name + ")";
                            string uniquePath = GetUniquePath(comp.gameObject);
                            var entry = new MissingScriptEntry(assetPath, displayName, uniquePath);
                            if (!output.Contains(entry))
                                output.Add(entry);
                        }
                    }
                }
            }
        }
        // Process children.
        foreach (Transform child in obj.transform)
        {
            CollectMissingReferences(child.gameObject, assetPath, output);
        }
    }

    private void ClearCache()
    {
        assetCache.Clear(); // Clear in-memory cache
        EditorPrefs.DeleteKey(CacheKey); // Remove saved cache from EditorPrefs
        Debug.Log("Cache cleared successfully.");
    }


    // Scans a single asset (prefab or scene) and returns the list of missing reference entries.
    // This method uses CollectMissingReferences to build a local list.
    private List<MissingScriptEntry> GetMissingReferencesForAsset(string assetPath)
    {
        List<MissingScriptEntry> output = new List<MissingScriptEntry>();
        try
        {
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject prefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefabRoot == null)
                {
                    Debug.LogWarning($"Skipping invalid prefab: {assetPath} (LoadAssetAtPath returned null)");
                    return output;
                }

                try
                {
                    CollectMissingReferences(prefabRoot, assetPath, output);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error collecting missing references in prefab: {assetPath}\n{ex}");
                }
            }

            else if (assetPath.EndsWith(".unity"))
            {
                Scene scene = new Scene();
                try
                {
                    scene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error opening scene: {assetPath}\n{ex}");
                }
                if (scene.isLoaded)
                {
                    try
                    {
                        foreach (GameObject root in scene.GetRootGameObjects())
                        {
                            CollectMissingReferences(root, assetPath, output);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error collecting missing references in scene: {assetPath}\n{ex}");
                    }
                    finally
                    {
                        try { EditorSceneManager.CloseScene(scene, true); } catch { }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"General error scanning asset {assetPath}: {ex}");
        }
        return output;
    }

    // Modified FindMissingScripts() that uses caching.
    private void FindMissingScripts()
    {
        missingScriptEntries.Clear();
        ignoredItemsFound = 0;
        assetFoldoutStates.Clear();
        ignoreFoldoutStates.Clear();

        // Fetch all prefabs and scenes inside "Assets/"
        string[] prefabAndSceneGUIDs = AssetDatabase.FindAssets("t:Prefab t:Scene", new[] { "Assets" });
        string[] allAssets = prefabAndSceneGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToArray();

        // Populate scene selection dictionary (if not already initialized)
        if (sceneSelection.Count == 0)
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!sceneSelection.ContainsKey(scene.path))
                    sceneSelection[scene.path] = scene.enabled; // Default: use enabled state from Build Settings
            }
        }

        // If scene filtering is enabled, only scan selected scenes
        if (sceneSelection.Values.Contains(true)) // If any scenes are selected
        {
            allAssets = allAssets.Where(asset => !asset.EndsWith(".unity") || (sceneSelection.ContainsKey(asset) && sceneSelection[asset])).ToArray();
        }


        int totalAssets = allAssets.Length;
        int processedAssets = 0;

        try
        {
            foreach (string assetPath in allAssets)
            {
                processedAssets++;

                if (skipTestingScenes && assetPath.StartsWith("Assets/Scenes/Testing/"))
                    continue;
                if (skipTrashScenes && assetPath.StartsWith("Assets/Scenes/Trash/"))
                    continue;

                DateTime fileTime = System.IO.File.GetLastWriteTime(assetPath);
                List<MissingScriptEntry> assetEntries = null;

                if (assetCache.TryGetValue(assetPath, out CacheEntry cache) && cache.lastWriteTime == fileTime)
                {
                    assetEntries = cache.entries;
                }
                else
                {
                    assetEntries = GetMissingReferencesForAsset(assetPath);
                    assetCache[assetPath] = new CacheEntry { lastWriteTime = fileTime, entries = assetEntries };
                }

                if (assetEntries != null && assetEntries.Count > 0)
                {
                    missingScriptEntries.AddRange(assetEntries);
                }

                float progress = (float)processedAssets / totalAssets;
                EditorUtility.DisplayProgressBar("Finding Missing Scripts",
                    $"Processing asset {processedAssets}/{totalAssets}", progress);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during scanning: {ex}");
        }
        finally
        {
            EditorUtility.ClearProgressBar(); // Always clears the progress bar, even if an error occurs.
        }

        SaveCacheToDisk();
        Repaint();
    }

    #endregion
    #region Jump/Select
    // Uses the unique path to locate the correct GameObject.
    private GameObject FindGameObjectByUniquePath(GameObject root, string uniquePath)
    {
        string[] parts = uniquePath.Split('/');
        if (parts.Length == 0)
            return null;

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

    // Updated jump for prefabs: first check for an open prefab stage.
    private void OpenPrefabAndSelect(string assetPath, string uniquePath)
    {
        PrefabStage currentStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (currentStage != null)
        {
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

    // Updated jump for scenes: if the active scene matches, use it; otherwise, open the scene.
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

    #region Replacement
    // Processes an entire asset (prefab or scene) by asset path.
    // It loads the asset, iterates over every GameObject in its hierarchy (including inactive ones),
    // removes ALL missing references, and if any were removed, adds the replacement component.
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

    // Iterates over every GameObject (including inactive ones) in the asset hierarchy.
    // For each GameObject, repeatedly calls RemoveMonoBehavioursWithMissingScript (with a cap)
    // and if any were removed, adds the replacement component.
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
    #endregion

    #region Load/Save
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

    private const string CacheKey = "MissingScriptsCache";

    private void SaveCacheToDisk()
    {
        try
        {
            List<string> serializedEntries = new List<string>();
            foreach (var kvp in assetCache)
            {
                string assetPath = kvp.Key;
                DateTime lastWriteTime = kvp.Value.lastWriteTime;
                List<string> entryData = new List<string>();

                foreach (var entry in kvp.Value.entries)
                {
                    entryData.Add($"{entry.assetPath}|{entry.objectName}|{entry.uniquePath}");
                }

                string serialized = $"{assetPath}::{lastWriteTime.ToBinary()}::{string.Join(";", entryData)}";
                serializedEntries.Add(serialized);
            }

            string serializedCache = string.Join("\n", serializedEntries);
            EditorPrefs.SetString(CacheKey, serializedCache);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save cache: {ex.Message}");
        }
    }

    private void LoadCacheFromDisk()
    {
        assetCache.Clear(); // Clear existing cache

        try
        {
            string serializedCache = EditorPrefs.GetString(CacheKey, "");
            if (!string.IsNullOrEmpty(serializedCache))
            {
                string[] entries = serializedCache.Split('\n');
                foreach (string entry in entries)
                {
                    if (!string.IsNullOrEmpty(entry))
                    {
                        string[] parts = entry.Split(new[] { "::" }, StringSplitOptions.None);
                        if (parts.Length == 3)
                        {
                            string assetPath = parts[0];
                            DateTime lastWriteTime = DateTime.FromBinary(long.Parse(parts[1]));
                            List<MissingScriptEntry> assetEntries = new List<MissingScriptEntry>();

                            string[] entryData = parts[2].Split(';');
                            foreach (string data in entryData)
                            {
                                string[] itemParts = data.Split('|');
                                if (itemParts.Length == 3)
                                {
                                    assetEntries.Add(new MissingScriptEntry(itemParts[0], itemParts[1], itemParts[2]));
                                }
                            }

                            assetCache[assetPath] = new CacheEntry { lastWriteTime = lastWriteTime, entries = assetEntries };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load cache: {ex.Message}");
        }
    }

    #endregion
}
