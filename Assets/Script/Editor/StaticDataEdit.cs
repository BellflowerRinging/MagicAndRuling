using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class StaticDataEdit : GameDataEditWindow
{
    [MenuItem("MagicAndRuling/StaticDataEdit")]

    private static void ShowWindow()
    {
        GetWindow<StaticDataEdit>("StaticDataEdit").Show();
        Input.imeCompositionMode = IMECompositionMode.On;
    }

    public override void SelectDataManager()
    {
        DataManager = Game.StaticDataManager;
    }
}