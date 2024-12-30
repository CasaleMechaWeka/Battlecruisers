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
        string[] allPrefabs = AssetDatabase.GetAllAssetPaths();
        foreach (string prefabPath in allPrefabs)
        {
            if (prefabPath.EndsWith(".prefab") || prefabPath.EndsWith(".unity"))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (obj != null && CheckForMissingScripts(obj))
                {
                    Debug.Log($"Missing script in: {prefabPath}", obj);
                    missingCount++;
                }
            }
        }

        Debug.Log($"Total assets with missing scripts: {missingCount}");
    }

    private static bool CheckForMissingScripts(GameObject obj)
    {
        bool hasMissingScripts = false;

        Component[] components = obj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                hasMissingScripts = true;
            }
        }

        foreach (Transform child in obj.transform)
        {
            if (CheckForMissingScripts(child.gameObject))
            {
                hasMissingScripts = true;
            }
        }

        return hasMissingScripts;
    }
}
