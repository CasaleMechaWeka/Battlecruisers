using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class MissingScriptsFinder : EditorWindow
{
    private struct MissingScriptEntry
    {
        public string assetPath;
        public GameObject gameObject;
    }

    private List<MissingScriptEntry> missingScriptEntries = new List<MissingScriptEntry>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptsFinder>("Missing Scripts Finder");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScripts();
        }

        if (missingScriptEntries.Count > 0)
        {
            EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (var entry in missingScriptEntries)
            {
                EditorGUILayout.BeginHorizontal();

                // Display asset path and GameObject name
                GUILayout.Label($"{entry.assetPath} -> {entry.gameObject.name}", GUILayout.Width(400));

                // Jump button to open the prefab and select the object
                if (GUILayout.Button("Jump", GUILayout.Width(80)))
                {
                    OpenPrefabAndSelect(entry.assetPath, entry.gameObject);
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

    private void FindMissingScripts()
    {
        missingScriptEntries.Clear();
        int missingCount = 0;

        // Check all prefabs and assets in the project
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssets)
        {
            if (assetPath.EndsWith(".prefab") || assetPath.EndsWith(".unity"))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (obj != null)
                {
                    missingCount += CheckForMissingScripts(obj, assetPath);
                }
            }
        }

        Debug.Log($"Total assets with missing scripts: {missingCount}");
        Repaint(); // Refresh the window
    }

    private int CheckForMissingScripts(GameObject obj, string assetPath)
    {
        int missingCount = 0;

        Component[] components = obj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                missingScriptEntries.Add(new MissingScriptEntry
                {
                    assetPath = assetPath,
                    gameObject = obj
                });

                missingCount++;
            }
        }

        foreach (Transform child in obj.transform)
        {
            missingCount += CheckForMissingScripts(child.gameObject, assetPath);
        }

        return missingCount;
    }

    private void OpenPrefabAndSelect(string assetPath, GameObject gameObject)
    {
        // Open the prefab in Prefab Mode
        PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(assetPath);
        if (prefabStage != null)
        {
            // Try to find the GameObject in the prefab instance
            GameObject prefabRoot = prefabStage.prefabContentsRoot;
            GameObject targetObject = FindGameObjectInPrefab(prefabRoot, gameObject.name);

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
}
