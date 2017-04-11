using UnityEngine; 
using UnityEditor; 
using System.Collections;

// https://feedback.unity3d.com/suggestions/please-let-us-use-layer-and-sortingorder-properties-on-non-sprite-gameobjects
[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererSortingLayersEditor : Editor 
{ 
	public override void OnInspectorGUI() 
	{ 
		base.OnInspectorGUI();

		MeshRenderer renderer = target as MeshRenderer;


		// Sorting layer name
		EditorGUILayout.BeginHorizontal(); 
		EditorGUI.BeginChangeCheck(); 

		string name = EditorGUILayout.TextField("Sorting Layer Name", renderer.sortingLayerName); 

		if (EditorGUI.EndChangeCheck()) 
		{ 
			renderer.sortingLayerName = name; 
		} 

		EditorGUILayout.EndHorizontal();


		// Sorting layer order
		EditorGUILayout.BeginHorizontal(); 
		EditorGUI.BeginChangeCheck(); 

		int order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder); 

		if(EditorGUI.EndChangeCheck()) 
		{ 
			renderer.sortingOrder = order; 
		} 

		EditorGUILayout.EndHorizontal(); 
	} 
}