using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ComponentSwap : EditorWindow
{
    [SerializeField] private MonoScript sourceScript;
    [SerializeField] private MonoScript targetScript;
    [SerializeField] private bool includeChildren = true;

    [MenuItem("Tools/Component Type Swapper")]
    private static void Open() => GetWindow<ComponentSwap>("Component Type Swapper");

    void OnGUI()
    {
        EditorGUILayout.LabelField("Swap Component Type (preserve serialized data)", EditorStyles.boldLabel);

        sourceScript = (MonoScript)EditorGUILayout.ObjectField("Source (to replace)", sourceScript, typeof(MonoScript), false);
        targetScript = (MonoScript)EditorGUILayout.ObjectField("Target (new type)", targetScript, typeof(MonoScript), false);
        includeChildren = EditorGUILayout.ToggleLeft("Include children of selected objects", includeChildren);

        bool valid = TypesValid(out Type srcType, out Type dstType);

        using (new EditorGUI.DisabledScope(!valid))
        {
            if (GUILayout.Button("Swap on Selection"))
            {
                SwapOnSelection(srcType, dstType, includeChildren);
            }

            if (GUILayout.Button("Swap in All Prefab Assets"))
            {
                SwapInAllPrefabs(srcType, dstType);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "This will:\n" +
            "1. Add the target component\n" +
            "2. Copy all matching serialized fields (by property path)\n" +
            "3. Remove the source component\n\n" +
            "Works for Derived → Base when the derived just inherits fields.\n" +
            "If a field doesn't exist on the target, it's skipped.",
            MessageType.Info);
    }

    bool TypesValid(out Type srcType, out Type dstType)
    {
        srcType = sourceScript ? sourceScript.GetClass() : null;
        dstType = targetScript ? targetScript.GetClass() : null;

        bool ok =
            srcType != null &&
            dstType != null &&
            typeof(Component).IsAssignableFrom(srcType) &&
            typeof(Component).IsAssignableFrom(dstType) &&
            srcType != dstType;

        return ok;
    }

    // ---- main ops ----------------------------------------------------------

    static void SwapOnSelection(Type srcType, Type dstType, bool includeChildren)
    {
        GameObject[] roots = Selection.gameObjects ?? Array.Empty<GameObject>();
        if (roots.Length == 0)
        {
            Debug.LogWarning("ComponentTypeSwapper: No GameObjects selected.");
            return;
        }

        Undo.IncrementCurrentGroup();
        int total = 0;

        foreach (GameObject root in roots)
        {
            IEnumerable<GameObject> targets = includeChildren
                ? root.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject)
                : new[] { root };

            foreach (GameObject go in targets)
                total += SwapOnGameObject(go, srcType, dstType);
        }

        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        if (total > 0) AssetDatabase.SaveAssets();
        Debug.Log($"ComponentTypeSwapper: swapped {total} component(s) in scene selection.");
    }

    static void SwapInAllPrefabs(Type srcType, Type dstType)
    {
        var guids = AssetDatabase.FindAssets("t:Prefab");
        int swappedTotal = 0;

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject root = PrefabUtility.LoadPrefabContents(path);
            if (!root) continue;

            int beforeCount = root.GetComponentsInChildren(srcType, true).Length;

            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                swappedTotal += SwapOnGameObject(t.gameObject, srcType, dstType);

            int afterCount = root.GetComponentsInChildren(srcType, true).Length;

            if (beforeCount != afterCount)
            {
                PrefabUtility.SaveAsPrefabAsset(root, path);
            }

            PrefabUtility.UnloadPrefabContents(root);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"ComponentTypeSwapper: swapped {swappedTotal} component(s) across prefab assets.");
    }

    static int SwapOnGameObject(GameObject go, Type srcType, Type dstType)
    {
        Component[] srcComponents = go.GetComponents(srcType);
        if (srcComponents == null || srcComponents.Length == 0)
            return 0;

        Undo.RegisterFullObjectHierarchyUndo(go, "Swap Component Type");

        int swapped = 0;

        foreach (Component src in srcComponents)
        {
            if (!src) continue;

            // 1. create new component of target type
            Component dst = go.AddComponent(dstType);
            if (!dst)
            {
                Debug.LogWarning($"ComponentTypeSwapper: Failed to add {dstType.Name} to {go.name}");
                continue;
            }

            // 2. copy serialized data
            CopySerializedData(src, dst);

            // 3. delete old
            Undo.DestroyObjectImmediate(src);

            swapped++;
        }

        EditorUtility.SetDirty(go);
        return swapped;
    }

    // ---- deep serialized copy ---------------------------------------------

    static void CopySerializedData(Component src, Component dst)
    {
        var soSrc = new SerializedObject(src);
        var soDst = new SerializedObject(dst);

        // Iterate over all visible properties on src (including nested),
        // and try to mirror them into dst if they exist.
        SerializedProperty prop = soSrc.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            // skip m_Script (different types, can't assign)
            if (prop.propertyPath == "m_Script")
                continue;

            SerializedProperty dstProp = soDst.FindProperty(prop.propertyPath);
            if (dstProp == null) continue;

            CopyPropertyRecursive(prop, dstProp);
        }

        soDst.ApplyModifiedPropertiesWithoutUndo();
    }

    static void CopyPropertyRecursive(SerializedProperty srcProp, SerializedProperty dstProp)
    {
        // Special case: strings should be copied directly.
        // Unity may report string as isArray == true, which fooled the old code.
        if (IsLeaf(srcProp))
        {
            CopyLeafValue(srcProp, dstProp);
            return;
        }

        // Real arrays (but not strings)
        if (srcProp.isArray && srcProp.propertyType != SerializedPropertyType.String)
        {
            dstProp.arraySize = srcProp.arraySize;
            for (int i = 0; i < srcProp.arraySize; i++)
            {
                SerializedProperty sEl = srcProp.GetArrayElementAtIndex(i);
                SerializedProperty dEl = dstProp.GetArrayElementAtIndex(i);
                CopyPropertyRecursive(sEl, dEl);
            }
            return;
        }

        // Structs / nested serializable classes / generics
        SerializedProperty srcIter = srcProp.Copy();
        SerializedProperty end = srcProp.GetEndProperty();

        bool enterChildren = true;
        while (srcIter.NextVisible(enterChildren) && !SerializedProperty.EqualContents(srcIter, end))
        {
            enterChildren = false;

            SerializedProperty childDst = dstProp.serializedObject.FindProperty(srcIter.propertyPath);
            if (childDst == null) continue;

            // avoid looping on same property
            if (childDst.propertyPath == dstProp.propertyPath)
                continue;

            CopyPropertyRecursive(srcIter, childDst);
        }
    }

    // helper to decide if something is a "leaf" we can just assign
    static bool IsLeaf(SerializedProperty p)
    {
        // Treat plain strings as leaf no matter what Unity says about isArray
        if (p.propertyType == SerializedPropertyType.String)
            return true;

        return !p.hasVisibleChildren &&
               !p.isArray &&
               p.propertyType != SerializedPropertyType.Generic;
    }


    static void CopyLeafValue(SerializedProperty src, SerializedProperty dst)
    {
        if (src.propertyType != dst.propertyType)
            return; // type mismatch -> skip

        switch (src.propertyType)
        {
            case SerializedPropertyType.Integer:
                dst.intValue = src.intValue;
                break;
            case SerializedPropertyType.Boolean:
                dst.boolValue = src.boolValue;
                break;
            case SerializedPropertyType.Float:
                dst.floatValue = src.floatValue;
                break;
            case SerializedPropertyType.String:
                dst.stringValue = src.stringValue;
                break;
            case SerializedPropertyType.Color:
                dst.colorValue = src.colorValue;
                break;
            case SerializedPropertyType.ObjectReference:
                dst.objectReferenceValue = src.objectReferenceValue;
                break;
            case SerializedPropertyType.LayerMask:
                dst.intValue = src.intValue;
                break;
            case SerializedPropertyType.Enum:
                dst.enumValueIndex = src.enumValueIndex;
                break;
            case SerializedPropertyType.Vector2:
                dst.vector2Value = src.vector2Value;
                break;
            case SerializedPropertyType.Vector3:
                dst.vector3Value = src.vector3Value;
                break;
            case SerializedPropertyType.Vector4:
                dst.vector4Value = src.vector4Value;
                break;
            case SerializedPropertyType.Rect:
                dst.rectValue = src.rectValue;
                break;
            case SerializedPropertyType.AnimationCurve:
                dst.animationCurveValue = src.animationCurveValue;
                break;
            case SerializedPropertyType.Bounds:
                dst.boundsValue = src.boundsValue;
                break;
            case SerializedPropertyType.Quaternion:
                dst.quaternionValue = src.quaternionValue;
                break;
#if UNITY_2020_1_OR_NEWER
            case SerializedPropertyType.Vector2Int:
                dst.vector2IntValue = src.vector2IntValue;
                break;
            case SerializedPropertyType.Vector3Int:
                dst.vector3IntValue = src.vector3IntValue;
                break;
            case SerializedPropertyType.RectInt:
                dst.rectIntValue = src.rectIntValue;
                break;
            case SerializedPropertyType.BoundsInt:
                dst.boundsIntValue = src.boundsIntValue;
                break;
#endif
            case SerializedPropertyType.ExposedReference:
                dst.exposedReferenceValue = src.exposedReferenceValue;
                break;
            case SerializedPropertyType.Hash128:
#if UNITY_2021_2_OR_NEWER
                dst.hash128Value = src.hash128Value;
#endif
                break;
            default:
                // fallback: do nothing for types we don't explicitly handle
                break;
        }
    }
}
