using UnityEditor;
using UnityEngine;

public class ReplaceNoneMaterialParticles : EditorWindow
{
    // The replacement material to assign to ParticleSystemRenderer components with no material
    public Material replacementMaterial;

    [MenuItem("Tools/Replace None Material Particle Systems")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceNoneMaterialParticles>("Replace Particle Materials");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace 'None' Materials in Particle Systems", EditorStyles.boldLabel);
        replacementMaterial = (Material)EditorGUILayout.ObjectField("Replacement Material", replacementMaterial, typeof(Material), false);

        if (GUILayout.Button("Replace Materials"))
        {
            if (replacementMaterial == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a replacement material.", "OK");
                return;
            }
            ReplaceMissingMaterials();
        }
    }

    private void ReplaceMissingMaterials()
    {
        // Find all prefab asset GUIDs in the project
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        int prefabsModified = 0;

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
                continue;

            bool prefabChanged = false;
            // Get all ParticleSystem components (including inactive ones) in the prefab hierarchy
            ParticleSystem[] particleSystems = prefab.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem ps in particleSystems)
            {
                // Get the ParticleSystemRenderer attached to the ParticleSystem
                ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
                if (psRenderer == null)
                    continue;

                // If the renderer's sharedMaterial is missing, assign the replacement material
                if (psRenderer.sharedMaterial == null)
                {
                    psRenderer.sharedMaterial = replacementMaterial;
                    prefabChanged = true;
                    Debug.Log($"Replaced missing material in prefab '{prefab.name}' at path '{path}'", prefab);
                }
            }

            // If any changes were made to this prefab, save the changes
            if (prefabChanged)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                prefabsModified++;
            }
        }

        EditorUtility.DisplayDialog("Replacement Complete", $"Replaced missing materials in {prefabsModified} prefab(s).", "OK");
    }
}
