using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DynamicDataEdit : GameDataEditWindow
{
    [MenuItem("MagicAndRuling/DynamicDataEdit")]

    private static void ShowWindow()
    {
        GetWindow<DynamicDataEdit>("DynamicDataEdit").Show();
        Input.imeCompositionMode = IMECompositionMode.On;
    }

    public override void SelectDataManager()
    {
        DataManager = Game.DynamicDataManager;
    }
}