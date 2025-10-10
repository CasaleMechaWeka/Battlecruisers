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
    struct MissingScriptEntry
    {
        public string assetPath;
        public string objectName;    // The GameObject's name or a label for the missing item.
        public string uniquePath;    // Full unique hierarchy path.

        public MissingScriptEntry(string assetPath, string objectName, string uniquePath)
        {
            this.assetPath = assetPath;
            this.objectName = objectName;
            this.uniquePath = uniquePath;
        }
    }
    #endregion

    #region Fields
    List<MissingScriptEntry> missingScriptEntries = new List<MissingScriptEntry>();
    List<MissingScriptEntry> ignoreList = new List<MissingScriptEntry>();
    Vector2 scrollPosition;
    bool viewIgnoreList = false;
    bool keepIgnoreList = true;
    bool skipTestingScenes = true;
    bool skipTrashScenes = true;
    bool filterScenes = true;
    bool filterPrefabs = true;

    bool filterMissingPrefabs = true;
    bool filterMissingComponents = true;
    bool filterMissingReferences = true;


    MonoScript replacementScript = null;

    Dictionary<string, bool> assetFoldoutStates = new Dictionary<string, bool>();
    Dictionary<string, bool> ignoreFoldoutStates = new Dictionary<string, bool>();

    const string IgnoreListKey = "MissingScriptsIgnoreList";

    // --- incremental scanning ---
    bool isScanning = false;
    List<string> scanAssets = new List<string>();
    int scanIndex = 0;
    double lastUIRefresh = 0.0;
    const float k_UI_REFRESH_INTERVAL = 0.5f;   // seconds
    float frameBudget = 0.1f;

    #endregion

    #region GUI
    [MenuItem("Tools/Find Missing Components")] // Renamed in menu
    public static void ShowWindow()
    {
        // Renamed in window title
        GetWindow<MissingScriptsFinder>("Missing Components Finder");
    }

    void OnEnable()
    {
        LoadIgnoreList();
        LoadCacheFromDisk();

        frameBudget = Mathf.Max(0.03f, 1f / Screen.currentResolution.refreshRate * .9f);
    }

    void OnDisable()
    {
        if (isScanning)
        {
            EditorApplication.update -= ScanStep;
            EditorUtility.ClearProgressBar();
        }

        if (keepIgnoreList)
            SaveIgnoreList();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find Missing Components"))
            FindMissingScripts();
        
        if (GUILayout.Button("Clear Cache", GUILayout.Width(100)))
            ClearCache();

        if (GUILayout.Button(viewIgnoreList ? $"View Main List ({missingScriptEntries.Count})" : $"View Ignore List ({ignoreList.Count})", GUILayout.Width(150)))
            viewIgnoreList = !viewIgnoreList;
            
        EditorGUILayout.EndHorizontal();

        //onlyScanBuildScenes = EditorGUILayout.Toggle("Only Scan Scenes in Build Settings", onlyScanBuildScenes);
        EditorGUILayout.BeginHorizontal();

        // Scenes button
        int sceneCount = missingScriptEntries.Count(e => e.assetPath.EndsWith(".unity"));
        GUIContent sceneButtonContent = new GUIContent(
            $"  {sceneCount}",
            EditorGUIUtility.IconContent("SceneAsset Icon").image,
            "Toggle scene scanning on/off"
        );
        bool newFilterScenes = GUILayout.Toggle(filterScenes, sceneButtonContent,
            "Button", GUILayout.Width(70), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        if (newFilterScenes != filterScenes)
        {
            filterScenes = newFilterScenes;
            Repaint();
        }

        // Prefabs button
        int prefabCount = missingScriptEntries.Count(e => e.assetPath.EndsWith(".prefab"));
        GUIContent prefabButtonContent = new GUIContent(
            $"  {prefabCount}",
            EditorGUIUtility.IconContent("Prefab Icon").image,
            "Toggle prefab scanning on/off"
        );
        bool newFilterPrefabs = GUILayout.Toggle(filterPrefabs, prefabButtonContent,
            "Button", GUILayout.Width(70), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        if (newFilterPrefabs != filterPrefabs)
        {
            filterPrefabs = newFilterPrefabs;
            Repaint();
        }

        // Missing Prefabs button
        GUIContent prefabBtnContent = new GUIContent("Missing Prefabs");
        bool newFilterMissingPrefabs = GUILayout.Toggle(filterMissingPrefabs, prefabBtnContent,
            "Button", GUILayout.Width(115));
        if (newFilterMissingPrefabs != filterMissingPrefabs)
        {
            filterMissingPrefabs = newFilterMissingPrefabs;
            Repaint();
        }

        // Missing Components button
        GUIContent compButtonContent = new GUIContent("Missing Components");
        bool newFilterMissingComponents = GUILayout.Toggle(filterMissingComponents, compButtonContent,
            "Button", GUILayout.Width(135));
        if (newFilterMissingComponents != filterMissingComponents)
        {
            filterMissingComponents = newFilterMissingComponents;
            Repaint();
        }

        // Missing References button
        GUIContent refsButtonContent = new GUIContent("Missing References");
        bool newFilterMissingReferences = GUILayout.Toggle(filterMissingReferences, refsButtonContent,
            "Button", GUILayout.Width(135));
        if (newFilterMissingReferences != filterMissingReferences)
        {
            filterMissingReferences = newFilterMissingReferences;
            Repaint();
        }

        // Script field (MonoScript)
        // Note: You can choose to show a label or not. For example, "Replacement Script" or just the field
        replacementScript = (MonoScript)EditorGUILayout.ObjectField(
            replacementScript,             // current value
            typeof(MonoScript),            // type filter
            false,                         // allowSceneObjects?
            GUILayout.Width(296)           // adjust as you like
        );

        // "Replace All" button
        bool hasAnyMissingComponents =
            missingScriptEntries.Any(e => e.objectName.StartsWith("Missing Component"));
        EditorGUI.BeginDisabledGroup(!hasAnyMissingComponents && replacementScript == null);

        if (GUILayout.Button(replacementScript ? "Replace All" : "Remove All", GUILayout.Width(80)))
            if (replacementScript != null)
                ReplaceAllMissing(replacementScript);
            else
                RemoveAllMissing();   // implement analogously if desired

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (viewIgnoreList)
            DisplayIgnoreList();
        else
            DisplayEntryList(missingScriptEntries, false);

        EditorGUILayout.EndScrollView();
    }

    // Helper: Recursively build a unique path for a GameObject.
    string GetUniquePath(GameObject go)
    {
        if (go.transform.parent == null)
            return go.name;

        string parentPath = GetUniquePath(go.transform.parent.gameObject);
        Transform parent = go.transform.parent;

        int totalSiblingsWithSameName = 0;

        foreach (Transform sibling in parent)
            if (sibling.gameObject.name == go.name)
                totalSiblingsWithSameName++;

        if (totalSiblingsWithSameName == 1)
            return $"{parentPath}/{go.name}";

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

    // This is where we handle filters and grouping.
    void DisplayEntryList(List<MissingScriptEntry> list, bool isIgnore)
    {
        if (list.Count == 0)
        {
            EditorGUILayout.LabelField(isIgnore
                ? "No ignored references found."
                : "No missing items found.");
            return;
        }

        // 1) Group by asset path
        Dictionary<string, List<MissingScriptEntry>> grouped = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (MissingScriptEntry entry in list)
        {
            if (!grouped.ContainsKey(entry.assetPath))
                grouped[entry.assetPath] = new List<MissingScriptEntry>();
            grouped[entry.assetPath].Add(entry);
        }

        // 2) Iterate groups
        foreach (KeyValuePair<string, List<MissingScriptEntry>> kvp in grouped)
        {
            string assetPath = kvp.Key;
            List<MissingScriptEntry> allEntries = kvp.Value;

            bool isScene = assetPath.EndsWith(".unity");
            bool isPrefab = assetPath.EndsWith(".prefab");

            // Apply scene/prefab filter
            if ((isScene && !filterScenes) || (isPrefab && !filterPrefabs))
                continue;

            // 3) Now filter each entry inside the group individually
            List<MissingScriptEntry> filteredEntries = new List<MissingScriptEntry>();
            foreach (MissingScriptEntry e in allEntries)
            {
                bool isMissingPrefab = e.objectName.StartsWith("Missing Prefab");
                bool isMissingComponent = e.objectName.StartsWith("Missing Component");
                bool isMissingReference = e.objectName.StartsWith("Missing Reference");

                // If the user wants to see missing components and this entry is a missing component,
                // or the user wants to see missing references and this entry is a missing reference,
                // then we keep this entry.
                if ((filterMissingComponents && isMissingComponent)
                    || (filterMissingReferences && isMissingReference)
                    || (filterMissingPrefabs && isMissingPrefab))
                {
                    filteredEntries.Add(e);
                }
            }

            // If no entries remain after the filter, skip this asset entirely
            if (filteredEntries.Count == 0)
                continue;

            bool hasMissingComponent = filteredEntries.Any(e => e.objectName.StartsWith("Missing Component"));

            // Get short name & icon
            string displayName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            GUIContent icon = isPrefab
                ? EditorGUIUtility.IconContent("Prefab Icon")
                : isScene
                    ? EditorGUIUtility.IconContent("SceneAsset Icon")
                    : null;

            // If we only have one entry left to show, just draw that row
            if (filteredEntries.Count == 1)
            {
                MissingScriptEntry single = filteredEntries[0];
                EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));

                if (icon != null)
                    GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));

                EditorGUILayout.LabelField($"{displayName} → {single.uniquePath}", GUILayout.ExpandWidth(true));

                if (hasMissingComponent)
                    if (replacementScript == null)
                        // REMOVE
                        if (GUILayout.Button("Remove", GUILayout.Width(60)))
                        {
                            RemoveMissingAssetGroup(single.assetPath);
                            Repaint();
                        }
                    else
                        // REPLACE
                        if (GUILayout.Button("Replace", GUILayout.Width(60)))
                        {
                            ReplaceMissingAssetGroup(single.assetPath, replacementScript);
                            Repaint();
                        }

                if (GUILayout.Button("Jump", GUILayout.Width(50)))
                    JumpToReference(single.assetPath, single.uniquePath);

                string btnText = isIgnore ? "Remove" : "Ignore";

                if (GUILayout.Button(btnText, GUILayout.Width(50)))
                    if (isIgnore)
                    {
                        ignoreList.Remove(single);
                        missingScriptEntries.Add(single);
                    }
                    else
                    {
                        if (!ignoreList.Contains(single))
                            ignoreList.Add(single);
                        missingScriptEntries.Remove(single);
                    }

                EditorGUILayout.EndHorizontal();
                continue;
            }

            // Otherwise, we have multiple entries in this group, so use a foldout
            EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (icon != null)
                GUILayout.Label(icon, GUILayout.Width(18), GUILayout.Height(18));

            if (!assetFoldoutStates.TryGetValue(assetPath, out bool expanded))
                assetFoldoutStates[assetPath] = false;

            assetFoldoutStates[assetPath] = EditorGUILayout.Foldout(
                assetFoldoutStates[assetPath],
                $"{displayName} ({filteredEntries.Count})",
                true
            );

            GUILayout.FlexibleSpace();

            if (hasMissingComponent)
                if (replacementScript == null)
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        RemoveMissingAssetGroup(assetPath);
                        Repaint();
                    }
                else
                    if (GUILayout.Button("Replace", GUILayout.Width(60)))
                    {
                        ReplaceMissingAssetGroup(assetPath, replacementScript);
                        Repaint();
                    }

            if (GUILayout.Button("Jump", GUILayout.Width(50)))
                OpenAssetAndSelectRoot(assetPath);

            string groupBtnText = isIgnore ? "Remove All" : "Ignore";

            if (GUILayout.Button(groupBtnText, GUILayout.Width(50)))
                if (isIgnore)
                {
                    foreach (MissingScriptEntry entry in filteredEntries)
                        missingScriptEntries.Add(entry);
                    ignoreList.RemoveAll(e => e.assetPath == assetPath);
                }
                else
                    foreach (MissingScriptEntry entry in filteredEntries)
                    {
                        if (!ignoreList.Contains(entry))
                            ignoreList.Add(entry);
                        missingScriptEntries.Remove(entry);
                    }

            EditorGUILayout.EndHorizontal();

            // Show each filtered entry in the foldout
            if (assetFoldoutStates[assetPath])
            {
                EditorGUI.indentLevel++;
                foreach (MissingScriptEntry entry in filteredEntries)
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.LabelField(entry.uniquePath, GUILayout.ExpandWidth(true));

                    if (GUILayout.Button("Jump", GUILayout.Width(50)))
                        JumpToReference(entry.assetPath, entry.uniquePath);

                    if (GUILayout.Button(isIgnore ? "Remove" : "Ignore", GUILayout.Width(50)))
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

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    void DisplayIgnoreList()
    {
        if (GUILayout.Button("Clear All", GUILayout.Width(80)))
            if (EditorUtility.DisplayDialog("Clear Ignore List", "Are you sure you want to clear the ignore list?", "Yes", "No"))
                ignoreList.Clear();
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayEntryList(ignoreList, true);
        EditorGUILayout.EndScrollView();
    }

    #endregion

    #region Caching and Unified Scanning
    class CacheEntry
    {
        public DateTime lastWriteTime;
        public List<MissingScriptEntry> entries;
    }

    Dictionary<string, CacheEntry> assetCache = new Dictionary<string, CacheEntry>();

    // Modified to label missing scripts as “Missing Component” and references as “Missing Reference: ...”
    void CollectMissingReferences(GameObject obj, string assetPath, List<MissingScriptEntry> output)
    {
        PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(obj);
        if (status == PrefabInstanceStatus.MissingAsset)
        {
            string uniquePath = GetUniquePath(obj);
            MissingScriptEntry entry = new MissingScriptEntry(assetPath, "Missing Prefab", uniquePath);
            if (!output.Contains(entry))
                output.Add(entry);
            return;    // no need to scan its components
        }

        Component[] comps = obj.GetComponents<Component>();
        foreach (Component comp in comps)
        {
            if (comp == null)
            {
                // Missing script → rename to “Missing Component”
                string uniquePath = GetUniquePath(obj);
                MissingScriptEntry entry = new MissingScriptEntry(assetPath, "Missing Component", uniquePath);
                if (!output.Contains(entry))
                    output.Add(entry);
            }
            else
            {
                // Check for missing references in serialized fields
                SerializedObject so = new SerializedObject(comp);
                SerializedProperty prop = so.GetIterator();
                while (prop.NextVisible(true))
                    if (prop.propertyType == SerializedPropertyType.ObjectReference
                     && prop.objectReferenceValue == null 
                     && prop.objectReferenceInstanceIDValue != 0)
                    {
                        // Label as “Missing Reference”
                        string displayName =
                            $"Missing Reference: {comp.gameObject.name} ({comp.GetType().Name}:{prop.name})";
                        string uniquePath = GetUniquePath(comp.gameObject);
                        MissingScriptEntry entry = new MissingScriptEntry(assetPath, displayName, uniquePath);
                        if (!output.Contains(entry))
                            output.Add(entry);
                    }
            }
        }
        // Recurse
        foreach (Transform child in obj.transform)
            CollectMissingReferences(child.gameObject, assetPath, output);
    }

    void ClearCache()
    {
        assetCache.Clear();
        EditorPrefs.DeleteKey(CacheKey);
        Debug.Log("Cache cleared successfully.");
    }

    List<MissingScriptEntry> GetMissingReferencesForAsset(string assetPath)
    {
        List<MissingScriptEntry> output = new List<MissingScriptEntry>();

        try
        {
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject prefabContents = null;
                try
                {
                    // this will throw if the prefab has !=1 root
                    prefabContents = PrefabUtility.LoadPrefabContents(assetPath);
                }
                catch (ArgumentException e)
                {
                    Debug.LogWarning($"Skipping prefab with multiple roots or invalid format: {assetPath}\n  {e.Message}");
                    return output;
                }

                // now do the normal scan
                CollectMissingReferences(prefabContents, assetPath, output);

                // unload the scratch scene
                PrefabUtility.UnloadPrefabContents(prefabContents);
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
                    try
                    {
                        foreach (GameObject root in scene.GetRootGameObjects())
                            CollectMissingReferences(root, assetPath, output);
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
        catch (Exception ex)
        {
            Debug.LogError($"General error scanning asset {assetPath}: {ex}");
        }
        return output;
    }

    void FindMissingScripts()
    {
        if (isScanning) return;            // already running

        // Use the original method to *prepare* everything – but ask it not to loop
        PrepareScanAssetList();            // builds scanAssets, sets scanIndex = 0
        isScanning = true;
        lastUIRefresh = EditorApplication.timeSinceStartup;
        EditorApplication.update += ScanStep;
    }

    void PrepareScanAssetList()
    {
        missingScriptEntries.Clear();
        assetFoldoutStates.Clear();
        ignoreFoldoutStates.Clear();

        IEnumerable<string> prefabPaths = AssetDatabase.FindAssets("t:MonoBehaviour t:Prefab", new[] { "Assets" })
                               .Select(AssetDatabase.GUIDToAssetPath);

        IEnumerable<string> buildScenePaths = EditorBuildSettings.scenes          // enabled build scenes only
                                .Where(s => s.enabled)
                                .Select(s => s.path);

        scanAssets = prefabPaths.Concat(buildScenePaths).ToList();

        // Apply the same filters you already have (testing, trash, etc.)
        scanAssets = scanAssets.Where(asset =>
            !(skipTestingScenes && asset.StartsWith("Assets/Scenes/Testing/")) &&
            !(skipTrashScenes && asset.StartsWith("Assets/Scenes/Trash/"))
        ).ToList();

        // … any other filters you used before …

        scanIndex = 0;
    }

    void ScanStep()
    {
        if (!isScanning)
            return;

        double frameStart = EditorApplication.timeSinceStartup;

        // Process assets until we've used ~8 ms this frame
        while (scanIndex < scanAssets.Count 
         && EditorApplication.timeSinceStartup - frameStart < frameBudget)
        {
            string assetPath = scanAssets[scanIndex];
            scanIndex++;

            DateTime fileTime = System.IO.File.GetLastWriteTime(assetPath);
            List<MissingScriptEntry> assetEntries = null;

            if (assetCache.TryGetValue(assetPath, out CacheEntry c) && c.lastWriteTime == fileTime)
                assetEntries = c.entries;
            else
            {
                assetEntries = GetMissingReferencesForAsset(assetPath);
                assetCache[assetPath] = new CacheEntry { lastWriteTime = fileTime, entries = assetEntries };
            }

            if (assetEntries != null && assetEntries.Count > 0)
                missingScriptEntries.AddRange(assetEntries);
        }

        // Progress bar
        float progress = (float)scanIndex / scanAssets.Count;
        EditorUtility.DisplayProgressBar("Finding Missing Components",
            $"Scanning {scanIndex}/{scanAssets.Count}", progress);

        // UI repaint throttled to 0.5 s
        if (EditorApplication.timeSinceStartup - lastUIRefresh > k_UI_REFRESH_INTERVAL)
        {
            lastUIRefresh = EditorApplication.timeSinceStartup;
            Repaint();
        }

        // Finished?
        if (scanIndex >= scanAssets.Count)
        {
            EditorUtility.ClearProgressBar();
            isScanning = false;
            EditorApplication.update -= ScanStep;
            SaveCacheToDisk();
            Repaint();              // final refresh
        }
    }

    #endregion

    #region Jump/Select
    GameObject FindGameObjectByUniquePath(GameObject root, string uniquePath)
    {
        string[] parts = uniquePath.Split('/');
        if (parts.Length == 0)
            return null;

        string rootName = parts[0];
        int bracket = rootName.IndexOf('[');
        
        if (bracket >= 0)
            rootName = rootName[..bracket];
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
                childName = part[..bIndex];
                int endBracket = part.IndexOf(']', bIndex);
                if (endBracket > bIndex)
                    int.TryParse(part.Substring(bIndex + 1, endBracket - bIndex - 1), out siblingIndex);
            }

            List<Transform> matches = new List<Transform>();
            foreach (Transform child in current.transform)
                if (child.gameObject.name == childName)
                    matches.Add(child);
            
            if (matches.Count <= siblingIndex)
                return null;
            current = matches[siblingIndex].gameObject;
        }
        return current;
    }

    void JumpToReference(string assetPath, string uniquePath)
    {
        if (assetPath.EndsWith(".prefab"))
            OpenPrefabAndSelect(assetPath, uniquePath);
        else if (assetPath.EndsWith(".unity"))
            OpenSceneAndSelect(assetPath, uniquePath);
    }

    void OpenPrefabAndSelect(string assetPath, string uniquePath)
    {
        PrefabStage currentStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (currentStage != null 
        && (string.IsNullOrEmpty(assetPath) || currentStage.assetPath == assetPath))
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
                Debug.LogWarning($"Could not find object with path {uniquePath} in prefab: {assetPath}");
        }
        else
            Debug.LogWarning($"Could not open prefab at {assetPath}");
    }

    void OpenSceneAndSelect(string scenePath, string uniquePath)
    {
        Scene currentScene = SceneManager.GetActiveScene();
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
            Debug.LogWarning($"Could not find object with path {uniquePath} in scene: {scenePath}");
    }

    void OpenAssetAndSelectRoot(string assetPath)
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
                    Debug.LogWarning($"Could not get prefab root for: {assetPath}");
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
    #endregion

    #region Replacement
    // Replacement logic was left untouched—still references “missing scripts”.
    // You can rename or adjust it as needed if you want.
    void ReplaceMissingAssetGroup(string assetPath, MonoScript replacementScript)
    {
        if (replacementScript == null) return;

        Type newType = replacementScript.GetClass();
        if (newType == null)
        {
            Debug.LogWarning("Replacement script does not have a valid class.");
            return;
        }

        /* --------------------------------------------------
           Remember what the user’s inspecting right now
        -------------------------------------------------- */
        UnityEngine.Object[] keepSelection = Selection.objects;
        Scene keepActiveScene = SceneManager.GetActiveScene();

        /* --------------------------------------------------
           PREFAB ASSETS
        -------------------------------------------------- */
        if (assetPath.EndsWith(".prefab"))
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null && stage.assetPath == assetPath)
            {
                // Already in Prefab Mode → edit what the inspector is showing
                GameObject root = stage.prefabContentsRoot;

                ProcessAssetForMissingReferences(root, newType);

                // Overwrite the asset on disk – works even if ‘root’ still has instance IDs
                PrefabUtility.SaveAsPrefabAsset(root, assetPath);

                // Optional: clear the dirty flag so the prefab’s title bar loses its asterisk
                stage.ClearDirtiness();
            }
            else
            {
                // Invisible scratch‑scene route for closed prefabs
                GameObject tmp = PrefabUtility.LoadPrefabContents(assetPath);
                ProcessAssetForMissingReferences(tmp, newType);
                PrefabUtility.SaveAsPrefabAsset(tmp, assetPath);
                PrefabUtility.UnloadPrefabContents(tmp);
            }
        }

        /* --------------------------------------------------
           SCENE ASSETS
        -------------------------------------------------- */
        else if (assetPath.EndsWith(".unity"))
        {
            // ❸ Is the scene already loaded?
            Scene targetScene = SceneManager.GetSceneByPath(assetPath);
            bool sceneWasLoaded = targetScene.isLoaded;

            if (!sceneWasLoaded)
                targetScene = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);

            foreach (GameObject root in targetScene.GetRootGameObjects())
                ProcessAssetForMissingReferences(root, newType);

            EditorSceneManager.MarkSceneDirty(targetScene);
            EditorSceneManager.SaveScene(targetScene);

            if (!sceneWasLoaded)
                EditorSceneManager.CloseScene(targetScene, true);
        }

        /* --------------------------------------------------
           Put everything back exactly as the user left it
        -------------------------------------------------- */
        Selection.objects = keepSelection;
        SceneManager.SetActiveScene(keepActiveScene);

        /* --------------------------------------------------
           Remove the fixed entries from the list and refresh
        -------------------------------------------------- */
        missingScriptEntries.RemoveAll(e => e.assetPath == assetPath);
        Repaint();
    }

    void ReplaceAllMissing(MonoScript replacementScript)
    {
        Dictionary<string, List<MissingScriptEntry>> groupedEntries = new Dictionary<string, List<MissingScriptEntry>>();
        foreach (MissingScriptEntry entry in missingScriptEntries)
        {
            // Skip pure missing‑reference rows
            if (!entry.objectName.StartsWith("Missing Component"))
                continue;

            if (!groupedEntries.ContainsKey(entry.assetPath))
                groupedEntries[entry.assetPath] = new List<MissingScriptEntry>();
            groupedEntries[entry.assetPath].Add(entry);
        }
        foreach (KeyValuePair<string, List<MissingScriptEntry>> kvp in groupedEntries)
            ReplaceMissingAssetGroup(kvp.Key, replacementScript);
    }

    #endregion
    #region Removal

    void RemoveMissingComponentsRecursively(GameObject root)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            RemoveMissingReferencesFromGameObject(t.gameObject);   // already does the actual removal
    }

    /// <summary>
    /// Strip missing components from the asset at <paramref name="assetPath"/> without adding anything.
    /// Mirrors ReplaceMissingAssetGroup but skips the AddComponent step.
    /// </summary>
    void RemoveMissingAssetGroup(string assetPath)
    {
        // remember user context
        UnityEngine.Object[] keepSelection = Selection.objects;
        Scene keepActiveScene = SceneManager.GetActiveScene();

        if (assetPath.EndsWith(".prefab"))
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null && stage.assetPath == assetPath)
            {
                RemoveMissingComponentsRecursively(stage.prefabContentsRoot);
                PrefabUtility.SaveAsPrefabAsset(stage.prefabContentsRoot, assetPath);
                stage.ClearDirtiness();
            }
            else
            {
                GameObject tmp = PrefabUtility.LoadPrefabContents(assetPath);
                RemoveMissingComponentsRecursively(tmp);
                PrefabUtility.SaveAsPrefabAsset(tmp, assetPath);
                PrefabUtility.UnloadPrefabContents(tmp);
            }
        }
        else if (assetPath.EndsWith(".unity"))
        {
            Scene target = SceneManager.GetSceneByPath(assetPath);
            bool wasLoaded = target.isLoaded;
            if (!wasLoaded)
                target = EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive);

            foreach (GameObject root in target.GetRootGameObjects())
                RemoveMissingComponentsRecursively(root);

            EditorSceneManager.MarkSceneDirty(target);
            EditorSceneManager.SaveScene(target);
            if (!wasLoaded)
                EditorSceneManager.CloseScene(target, true);
        }

        // restore context and refresh list
        Selection.objects = keepSelection;
        SceneManager.SetActiveScene(keepActiveScene);
        missingScriptEntries.RemoveAll(e =>
            e.assetPath == assetPath &&
            e.objectName.StartsWith("Missing Component"));
        Repaint();
    }

    void RemoveAllMissing()
    {
        // distinct list so we don’t hit the same prefab / scene twice
        string[] assetPaths = missingScriptEntries
            .Where(e => e.objectName.StartsWith("Missing Component"))
            .Select(e => e.assetPath)
            .Distinct()
            .ToArray();

        foreach (string path in assetPaths)
            RemoveMissingAssetGroup(path);   // already handles context & UI refresh

        // safety: make sure the list is cleared once everything is done
        missingScriptEntries.Clear();
        Repaint();
    }

    #endregion
    #region Preprocess

    void ProcessAssetForMissingReferences(GameObject root, Type newType)
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

    int RemoveMissingReferencesFromGameObject(GameObject go)
    {
        int totalRemoved = 0;
        int iteration = 0;
        int maxIteration = 10;
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
    void LoadIgnoreList()
    {
        ignoreList.Clear();
        string serializedList = EditorPrefs.GetString(IgnoreListKey, "");
        if (!string.IsNullOrEmpty(serializedList))
        {
            string[] entries = serializedList.Split(';');
            foreach (string entry in entries)
                if (!string.IsNullOrEmpty(entry))
                {
                    string[] parts = entry.Split('|');
                    if (parts.Length == 3)
                        ignoreList.Add(new MissingScriptEntry(parts[0], parts[1], parts[2]));
                }
        }
    }

    void SaveIgnoreList()
    {
        List<string> serializedEntries = new List<string>();
        foreach (MissingScriptEntry entry in ignoreList)
            serializedEntries.Add($"{entry.assetPath}|{entry.objectName}|{entry.uniquePath}");
        
        string serializedList = string.Join(";", serializedEntries);
        EditorPrefs.SetString(IgnoreListKey, serializedList);
    }

    const string CacheKey = "MissingScriptsCache";

    void SaveCacheToDisk()
    {
        try
        {
            List<string> serializedEntries = new List<string>();
            foreach (KeyValuePair<string, CacheEntry> kvp in assetCache)
            {
                string assetPath = kvp.Key;
                DateTime lastWriteTime = kvp.Value.lastWriteTime;
                List<string> entryData = new List<string>();

                foreach (MissingScriptEntry entry in kvp.Value.entries)
                    entryData.Add($"{entry.assetPath}|{entry.objectName}|{entry.uniquePath}");

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

    void LoadCacheFromDisk()
    {
        assetCache.Clear();
        try
        {
            string serializedCache = EditorPrefs.GetString(CacheKey, "");
            if (!string.IsNullOrEmpty(serializedCache))
            {
                string[] entries = serializedCache.Split('\n');
                foreach (string entry in entries)
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
                                    assetEntries.Add(new MissingScriptEntry(itemParts[0], itemParts[1], itemParts[2]));
                            }

                            assetCache[assetPath] = new CacheEntry { lastWriteTime = lastWriteTime, entries = assetEntries };
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
