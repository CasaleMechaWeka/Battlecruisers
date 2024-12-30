using UnityEditor;
using UnityEngine;

public class MissingScriptsFinder : EditorWindow
{
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
    }

    private static void FindMissingScripts()
    {
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
    }

    private static int CheckForMissingScripts(GameObject obj, string assetPath)
    {
        int missingCount = 0;

        Component[] components = obj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                // Log with a clickable reference to the specific GameObject
                Debug.LogWarning($"Missing script found on GameObject '{obj.name}' in asset: {assetPath}", obj);
                missingCount++;
            }
        }

        foreach (Transform child in obj.transform)
        {
            missingCount += CheckForMissingScripts(child.gameObject, assetPath);
        }

        return missingCount;
    }
}
