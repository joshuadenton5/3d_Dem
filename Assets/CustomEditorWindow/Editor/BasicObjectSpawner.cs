using UnityEditor;
using UnityEngine;

public class BasicObjectSpawner : EditorWindow
{
    [MenuItem("Tools/Basic Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BasicObjectSpawner));
    }

    private void OnGUI()
    {
        
    }
}
