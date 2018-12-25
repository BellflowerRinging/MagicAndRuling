using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuEditor
{

    // Use this for initialization
    [MenuItem("MagicAndRuling/Test")]
    private static void Test()
    {
        EditorWindow.GetWindow(typeof(GameDataEditWindow));
    }
}
