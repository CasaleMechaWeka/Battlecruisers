using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptsFinder : EditorWindow
{

    enum MissingType
    {
        MissingReference = 0,
        MissingComponent = 1,
        MissingPrefab = 2,
    }

    #region Data Structures
    // Stores a unique path for each missing reference.
    struct MissingScriptEntry
    {
        public string assetPath;
        public MissingType missingType;    // The GameObject's name or a label for the missing item.
        public string uniquePath;    // Full unique hierarchy path.

        public MissingScriptEntry(string assetPath, MissingType missingType, string uniquePath)
        {
            this.assetPath = assetPath;
            this.missingType = missingType;
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
    bool scanScenes = true;
    bool scanPrefabs = true;
    const bool StopAtFirstRefPerComponent = false;

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

    #region GUI Vars

    static Texture2D IconScene;
    static Texture2D IconPrefab;
    static Texture2D IconGameObject;

    static GUIContent GC_Scenes;
    static GUIContent GC_Prefabs;

    static readonly GUIContent GC_FindMissingComponents = EditorGUIUtility.TrTextContent("Find Missing Components");
    static readonly GUIContent GC_ClearCache = EditorGUIUtility.TrTextContent("Clear Cache");
    static readonly GUIContent GC_ViewList = new GUIContent();

    static readonly GUIContent GC_MissingPrefabs = new GUIContent("Missing Prefabs");
    static readonly GUIContent GC_MissingComponents = new GUIContent("Missing Components");
    static readonly GUIContent GC_MissingReferences = new GUIContent("Missing References");


    static readonly GUIContent GC_Remove = EditorGUIUtility.TrTextContent("Remove");
    static readonly GUIContent GC_Replace = EditorGUIUtility.TrTextContent("Replace");
    static readonly GUIContent GC_Jump = EditorGUIUtility.TrTextContent("Jump");
    static readonly GUIContent GC_Ignore = EditorGUIUtility.TrTextContent("Ignore");
    static readonly GUIContent GC_RemoveAll = EditorGUIUtility.TrTextContent("Remove All");


    static readonly GUILayoutOption[] OPT_ROW = { GUILayout.Height(EditorGUIUtility.singleLineHeight) };
    static readonly GUILayoutOption[] OPT_ICON = { GUILayout.Width(18f), GUILayout.Height(18f) };
    static readonly GUILayoutOption[] OPT_W50 = { GUILayout.Width(50f) };
    static readonly GUILayoutOption[] OPT_W60 = { GUILayout.Width(60f) };
    static readonly GUILayoutOption[] OPT_W70 = { GUILayout.Width(70f) };
    static readonly GUILayoutOption[] OPT_W135 = { GUILayout.Width(135f) };

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
        IconScene = (Texture2D)EditorGUIUtility.IconContent("SceneAsset Icon").image;
        IconPrefab = (Texture2D)EditorGUIUtility.IconContent("Prefab Icon").image;
        IconGameObject = (Texture2D)EditorGUIUtility.IconContent("GameObject Icon").image;

        GC_Scenes = new GUIContent("", IconScene, "Toggle scene scanning on/off");
        GC_Prefabs = new GUIContent("", IconPrefab, "Toggle prefab scanning on/off");

        LoadIgnoreList();
        LoadCacheFromDisk();

        frameBudget = Mathf.Max(0.03f, 1f / (float)Screen.currentResolution.refreshRateRatio.value * .9f);
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
        if (GUILayout.Button(GC_FindMissingComponents))
            FindMissingScripts();

        if (GUILayout.Button(GC_ClearCache, GUILayout.Width(100)))
            ClearCache();

        GC_ViewList.text = viewIgnoreList
            ? $"View Main List ({missingScriptEntries.Count})"
            : $"View Ignore List ({ignoreList.Count})";

        if (GUILayout.Button(GC_ViewList, GUILayout.Width(150)))
            viewIgnoreList = !viewIgnoreList;

        EditorGUILayout.EndHorizontal();

        //onlyScanBuildScenes = EditorGUILayout.Toggle("Only Scan Scenes in Build Settings", onlyScanBuildScenes);
        EditorGUILayout.BeginHorizontal();

        // Scenes button
        int sceneCount = missingScriptEntries.Count(e => e.assetPath.EndsWith(".unity"));
        GC_Scenes.text = $"  {sceneCount}";
        DrawFilterButton(ref scanScenes, GC_Scenes, GUILayout.Width(70), GUILayout.Height(EditorGUIUtility.singleLineHeight));

        // Prefabs button
        int prefabCount = missingScriptEntries.Count(e => e.assetPath.EndsWith(".prefab"));
        GC_Prefabs.text = $"  {prefabCount}";
        DrawFilterButton(ref scanPrefabs, GC_Prefabs, GUILayout.Width(70), GUILayout.Height(EditorGUIUtility.singleLineHeight));

        DrawFilterButton(ref filterMissingPrefabs,    GC_MissingPrefabs,    GUILayout.Width(115));
        DrawFilterButton(ref filterMissingComponents, GC_MissingComponents, OPT_W135);
        DrawFilterButton(ref filterMissingReferences, GC_MissingReferences, OPT_W135);

        // Script field (MonoScript)
        // Note: You can choose to show a label or not. For example, "Replacement Script" or just the field
        replacementScript = (MonoScript)EditorGUILayout.ObjectField(
            replacementScript,             // current value
            typeof(MonoScript),            // type filter
            false                          // allowSceneObjects?
        );

        // "Replace All" button
        bool hasAnyMissingComponents =
            missingScriptEntries.Any(e => e.missingType == MissingType.MissingComponent);
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

    // This is where we handle filters and grouping.
    void DisplayEntryList(List<MissingScriptEntry> list, bool isIgnore)
    {
        if (list.Count == 0)
        {
            EditorGUILayout.LabelField("No missing items found.");
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

            bool isScene = assetPath.EndsWith(".unity", StringComparison.OrdinalIgnoreCase);
            bool isPrefab = assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase);

            // apply scene/prefab filter
            if ((isScene && !scanScenes) || (isPrefab && !scanPrefabs))
                continue;

            // per-type filter (FIXED mapping: Component -> MissingComponent, Reference -> MissingReference)
            List<MissingScriptEntry> filteredEntries = new List<MissingScriptEntry>(allEntries.Count);
            foreach (MissingScriptEntry e in allEntries)
                if (PassesTypeFilter(e.missingType, filterMissingComponents, filterMissingReferences, filterMissingPrefabs))
                    filteredEntries.Add(e);

            if (filteredEntries.Count == 0) continue;

            bool hasMissingComponent = false;
            for (int i = 0; i < filteredEntries.Count; i++)
                if (filteredEntries[i].missingType == MissingType.MissingComponent) { hasMissingComponent = true; break; }

            string displayName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            Texture2D iconTex = isPrefab ? IconPrefab : isScene ? IconScene : null;

            // single row
            if (filteredEntries.Count == 1)
            {
                MissingScriptEntry single = filteredEntries[0];
                EditorGUILayout.BeginHorizontal(OPT_ROW);
                if (iconTex != null) GUILayout.Label(iconTex, OPT_ICON);
                EditorGUILayout.LabelField(FormatPathRootArrow(single.uniquePath), GUILayout.ExpandWidth(true));

                if (single.missingType == MissingType.MissingComponent)
                    DrawRemoveAndReplace(single.assetPath, replacementScript);
                DrawJumpForEntry(single);
                DrawIgnoreForEntry(single, isIgnore);

                EditorGUILayout.EndHorizontal();
                continue;
            }

            // group header (foldout)
            EditorGUILayout.BeginHorizontal(OPT_ROW);
            if (iconTex != null) GUILayout.Label(iconTex, OPT_ICON);

            bool expanded = assetFoldoutStates.TryGetValue(assetPath, out bool wasExpanded) && wasExpanded;
            bool newExpanded = EditorGUILayout.Foldout(expanded, $"{displayName} ({filteredEntries.Count})", true);

            // Always store back so the key exists for next frame
            assetFoldoutStates[assetPath] = newExpanded;

            GUILayout.FlexibleSpace();

            if (hasMissingComponent)
                DrawRemoveAndReplace(assetPath, replacementScript);
            DrawJumpForAsset(assetPath);
            DrawIgnoreForGroup(assetPath, filteredEntries, isIgnore);

            EditorGUILayout.EndHorizontal();

            // Use the local variable, not the dictionary indexer
            if (assetFoldoutStates[assetPath])
            {
                EditorGUI.indentLevel++; // you already do this for children
                for (int i = 0; i < filteredEntries.Count; i++)
                {
                    MissingScriptEntry entry = filteredEntries[i];
                    string sub = SubPathAfterRoot(entry.uniquePath); // from earlier

                    EditorGUILayout.BeginHorizontal(OPT_ROW);

                    // indent the whole child row (icon + label + buttons)
                    float indentPx = 15f * EditorGUI.indentLevel; // Unity's standard indent width ~15
                    GUILayout.Space(indentPx);

                    // indented GameObject icon
                    GUILayout.Label(IconGameObject, OPT_ICON);

                    // avoid double-indenting the label (since we spaced manually)
                    int oldIndent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.LabelField(string.IsNullOrEmpty(sub) ? displayName : sub, GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel = oldIndent;

                    if (entry.missingType == MissingType.MissingComponent)
                        DrawRemoveAndReplace(entry.assetPath, replacementScript);

                    DrawJumpForEntry(entry);
                    DrawIgnoreForEntry(entry, isIgnore);

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    static string FormatPathRootArrow(string uniquePath)
    {
        if (string.IsNullOrEmpty(uniquePath)) return string.Empty;
        int i = uniquePath.IndexOf('/');
        if (i < 0) return uniquePath;                // root only
        return uniquePath[..i] + " → " + uniquePath[(i + 1)..];
    }

    static string SubPathAfterRoot(string uniquePath)
    {
        int i = uniquePath.IndexOf('/');
        return i < 0 ? string.Empty : uniquePath.Substring(i + 1);
    }

    void DrawRemoveAndReplace(string assetPath, MonoScript replacementScript)
    {
        // Remove (always enabled)
        if (GUILayout.Button(GC_Remove, OPT_W60))
        {
            RemoveMissingAssetGroup(assetPath);
            Repaint();
        }

        // Replace (disabled when no script selected)
        using (new EditorGUI.DisabledScope(replacementScript == null))
        {
            if (GUILayout.Button(GC_Replace, OPT_W60))
            {
                ReplaceMissingAssetGroup(assetPath, replacementScript);
                Repaint();
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool PassesTypeFilter(MissingType t, bool fComp, bool fRef, bool fPrefab) =>
       (fComp && t == MissingType.MissingComponent)
    || (fRef && t == MissingType.MissingReference)
    || (fPrefab && t == MissingType.MissingPrefab);

    void DrawJumpForEntry(in MissingScriptEntry e)
    {
        if (GUILayout.Button(GC_Jump, OPT_W50))
            JumpToReference(e.assetPath, e.uniquePath);
    }

    void DrawJumpForAsset(string assetPath)
    {
        if (GUILayout.Button(GC_Jump, OPT_W50))
            OpenAssetAndSelectRoot(assetPath);
    }

    void DrawIgnoreForEntry(in MissingScriptEntry e, bool isIgnore)
    {
        if (GUILayout.Button(isIgnore ? GC_Remove : GC_Ignore, OPT_W50))
            if (isIgnore)
            {
                ignoreList.Remove(e);
                missingScriptEntries.Add(e);
            }
            else
            {
                if (!ignoreList.Contains(e))
                    ignoreList.Add(e);
                missingScriptEntries.Remove(e);
            }
    }

    void DrawIgnoreForGroup(string assetPath, List<MissingScriptEntry> entries, bool isIgnore)
    {
        if (GUILayout.Button(isIgnore ? GC_RemoveAll : GC_Ignore, OPT_W50))
            if (isIgnore)
            {
                foreach (MissingScriptEntry e in entries) missingScriptEntries.Add(e);
                ignoreList.RemoveAll(e => e.assetPath == assetPath);
            }
            else
                foreach (MissingScriptEntry e in entries)
                {
                    if (!ignoreList.Contains(e)) ignoreList.Add(e);
                    missingScriptEntries.Remove(e);
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

    void DrawFilterButton(ref bool flag, GUIContent content, params GUILayoutOption[] opts)
    {
        EditorGUI.BeginChangeCheck();
        bool v = GUILayout.Toggle(flag, content, "Button", opts);
        if (EditorGUI.EndChangeCheck())
        {
            flag = v; 
            Repaint(); 
        }
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
        // Per-asset de-dupe: "path|type"
        HashSet<string> seen = new HashSet<string>(128);
        Traverse(obj, assetPath, output, seen, obj.name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static string EntryKey(string uniquePath, MissingType t) => uniquePath + "|" + (int)t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void AddOnce(List<MissingScriptEntry> dst, HashSet<string> seen, string assetPath, string uniquePath, MissingType kind)
    {
        if (seen.Add(EntryKey(uniquePath, kind)))
            dst.Add(new MissingScriptEntry(assetPath, kind, uniquePath));
    }

    static readonly List<Component> s_Comps = new List<Component>(8);

    void Traverse(GameObject go, string assetPath, List<MissingScriptEntry> output, HashSet<string> seen, string currentPath)
    {
        // Missing prefab instance? Record once and skip this subtree.
        if (PrefabUtility.GetPrefabInstanceStatus(go) == PrefabInstanceStatus.MissingAsset)
        {
            AddOnce(output, seen, assetPath, currentPath, MissingType.MissingPrefab);
            return;
        }

        // Components on this GameObject

        s_Comps.Clear();
        go.GetComponents(s_Comps);              // no array alloc

        bool hasMissingComponentHere = false;
        for (int i = 0; i < s_Comps.Count; i++)
        {
            Component comp = s_Comps[i];
            if (comp == null) { hasMissingComponentHere = true; continue; }
            if (comp is Transform) continue;

            var so = new SerializedObject(comp);
            SerializedProperty it = so.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.propertyType == SerializedPropertyType.ObjectReference &&
                    it.objectReferenceInstanceIDValue != 0 &&
                    it.objectReferenceValue == null)
                {
                    AddOnce(output, seen, assetPath, currentPath, MissingType.MissingReference);
                    if (StopAtFirstRefPerComponent) break;  // this is here for a reason. Don't remove it just because you think you can
                }
            }
        }
        if (hasMissingComponentHere)
            AddOnce(output, seen, assetPath, currentPath, MissingType.MissingComponent);

            
        // Children: compute stable unique paths in one pass
        Transform tr = go.transform;
        int childCount = tr.childCount;
        if (childCount == 0) return;
        // Count how many children share each name
        Dictionary<string, int> counts = new Dictionary<string, int>(childCount);
        for (int i = 0; i < childCount; i++)
        {
            Transform child = tr.GetChild(i);
            counts.TryGetValue(child.name, out int c);
            counts[child.name] = c + 1;
        }
        // Assign indices for duplicate names as we walk
        Dictionary<string, int> used = new Dictionary<string, int>(counts.Count);
        for (int i = 0; i < childCount; i++)
        {
            Transform child = tr.GetChild(i);
            string name = child.name;
            counts.TryGetValue(name, out int total);
            string childPath;
            if (total <= 1)
            {
                childPath = currentPath + "/" + name;
            }
            else
            {
                used.TryGetValue(name, out int idx);
                childPath = $"{currentPath}/{name}[{idx}]";
                used[name] = idx + 1;
            }
            Traverse(child.gameObject, assetPath, output, seen, childPath);
        }
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
        string current =
            (scanIndex > 0 && scanIndex <= scanAssets.Count)
                ? System.IO.Path.GetFileName(scanAssets[scanIndex - 1])
                : string.Empty;

        bool canceled = EditorUtility.DisplayCancelableProgressBar(
            "Finding Missing Components",
            $"Scanning {scanIndex}/{scanAssets.Count}  {current}",
            progress);

        if (canceled)
        {
            CancelScan();
            return;
        }

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

    void CancelScan()
    {
        isScanning = false;
        EditorApplication.update -= ScanStep;
        EditorUtility.ClearProgressBar();
        SaveCacheToDisk();   // keep whatever we’ve already scanned
        Repaint();
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
            if (!(entry.missingType == MissingType.MissingComponent))
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
            e.missingType == MissingType.MissingComponent);
        Repaint();
    }

    void RemoveAllMissing()
    {
        // distinct list so we don’t hit the same prefab / scene twice
        string[] assetPaths = missingScriptEntries
            .Where(e => e.missingType == MissingType.MissingComponent)
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
        if (string.IsNullOrEmpty(serializedList))
            return;

        string[] entries = serializedList.Split(';');
        foreach (string entry in entries)
        {
            if (string.IsNullOrEmpty(entry)) continue;

            string[] parts = entry.Split('|');
            if (parts.Length != 3) continue;

            string assetPath = parts[0];
            string typeToken = parts[1];
            string uniquePath = parts[2];

            MissingType kind;

            // New format: integer enum value
            if (int.TryParse(typeToken, out int intVal) && Enum.IsDefined(typeof(MissingType), intVal))
                kind = (MissingType)intVal;
            else
            {
                // Back-compat: old format stored a label like
                // "Missing Component", "Missing Prefab", or "Missing Reference: ...".
                if (typeToken.StartsWith("Missing Prefab", StringComparison.Ordinal))
                    kind = MissingType.MissingPrefab;
                else if (typeToken.StartsWith("Missing Component", StringComparison.Ordinal))
                    kind = MissingType.MissingComponent;
                else if (typeToken.StartsWith("Missing Reference", StringComparison.Ordinal))
                    kind = MissingType.MissingReference;
                else
                    // Safe default
                    kind = MissingType.MissingReference;
            }

            ignoreList.Add(new MissingScriptEntry(assetPath, kind, uniquePath));
        }
    }


    void SaveIgnoreList()
    {
        List<string> serializedEntries = new List<string>(ignoreList.Count);
        foreach (MissingScriptEntry entry in ignoreList)
            serializedEntries.Add($"{entry.assetPath}|{(int)entry.missingType}|{entry.uniquePath}");

        string serializedList = string.Join(";", serializedEntries);
        EditorPrefs.SetString(IgnoreListKey, serializedList);
    }

    const string CacheKey = "MissingScriptsCache";

    void SaveCacheToDisk()
    {
        try
        {
            List<string> serializedEntries = new List<string>(assetCache.Count);
            foreach (KeyValuePair<string, CacheEntry> kvp in assetCache)
            {
                string assetPath = kvp.Key;
                long lastWrite = kvp.Value.lastWriteTime.ToBinary();

                List<string> entryData = new List<string>(kvp.Value.entries?.Count ?? 0);
                if (kvp.Value.entries != null)
                {
                    foreach (MissingScriptEntry entry in kvp.Value.entries)
                        entryData.Add($"{entry.assetPath}|{(int)entry.missingType}|{entry.uniquePath}");
                }

                string serialized = $"{assetPath}::{lastWrite}::{string.Join(';', entryData)}";
            serializedEntries.Add(serialized);
        }

        string serializedCache = string.Join('\n', serializedEntries);
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
            if (string.IsNullOrEmpty(serializedCache))
                return;

            // If we detect the *old* cache format (non-integer in type slot), nuke it and bail.
            bool oldFormatDetected = false;

            foreach (string line in serializedCache.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split(new[] { "::" }, StringSplitOptions.None);
                if (parts.Length != 3) { oldFormatDetected = true; break; }

                string assetPath = parts[0];

                if (!long.TryParse(parts[1], out long binTime))
                {
                    oldFormatDetected = true;
                    break;
                }
                DateTime lastWriteTime = DateTime.FromBinary(binTime);

                List<MissingScriptEntry> assetEntries = new List<MissingScriptEntry>();

                string payload = parts[2];
                if (!string.IsNullOrEmpty(payload))
                {
                    foreach (string item in payload.Split(';'))
                    {
                        if (string.IsNullOrEmpty(item)) continue;

                        string[] itemParts = item.Split('|');
                        if (itemParts.Length != 3) { oldFormatDetected = true; break; }

                        // New format: assetPath | <int MissingType> | uniquePath
                        if (!int.TryParse(itemParts[1], out int typeVal))
                        {
                            oldFormatDetected = true;
                            break;
                        }

                        MissingType kind = (MissingType)typeVal;
                        string eAssetPath = itemParts[0];
                        string uniquePath = itemParts[2];

                        assetEntries.Add(new MissingScriptEntry(eAssetPath, kind, uniquePath));
                    }
                }

                if (oldFormatDetected) break;

                assetCache[assetPath] = new CacheEntry
                {
                    lastWriteTime = lastWriteTime,
                    entries = assetEntries
                };
            }

            if (oldFormatDetected)
            {
                assetCache.Clear();
                EditorPrefs.DeleteKey(CacheKey); // discard old cache outright
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load cache: {ex.Message}");
        }
    }
    #endregion
}
