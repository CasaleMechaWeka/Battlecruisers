using UnityEngine;
using UnityEditor;

public class TestEditorWindow : EditorWindow
{
    [MenuItem("Tools/Test ChainBattle Editor")]
    static void TestOpen()
    {
        var window = GetWindow<ChainBattleEditorWindow>();
        window.titleContent = new GUIContent("ChainBattle Editor");
        window.Show();
    }
}
