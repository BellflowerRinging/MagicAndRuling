using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GameDataEditWindow : EditorWindow
{

    private static void ShowWindow()
    {
        GetWindow<GameDataEditWindow>("GameDataEdit").Show();
        Input.imeCompositionMode = IMECompositionMode.On;
    }

    protected Vector2 ScrollViewV2 = new Vector2(0, 0);
    protected int SelectIetmIndex = 0;
    protected static string[] DataNames;
    public string[] fields = new string[30];
    public string[] InputStrings = new string[30];
    protected List<GameData> GameDatas;
    protected List<GameData> DynamicAndStaicGameDatas;

    public int SelectGameDataTypeIndex = 0;
    public int ShowGameDataTypeIndex = 0;
    public int LastShowGameDataTypeIndex = -1;


    protected int LastSelectIetmIndex = -1;
    protected GUIStyle TextCenterStyle = new GUIStyle();

    void OnEnable()
    {
        TextCenterStyle.alignment = TextAnchor.MiddleCenter;
        Game.InitDataManager();
        SelectDataManager();
        DynamicAndStaicGameDatas = new List<GameData>();
        DynamicAndStaicGameDatas.AddRange(Game.DynamicDataManager.GetGameDatas());
        DynamicAndStaicGameDatas.AddRange(Game.StaticDataManager.GetGameDatas());
    }
    protected DataManager DataManager;
    public virtual void SelectDataManager()
    {
        Debug.Log("1");
    }

    private void OnGUI()
    {
        Game.DynamicDataManager.UpdataToContent();

        GUILayout.Label("All Game Data List", EditorStyles.boldLabel);

        if (GUILayout.Button("重新载入"))
        {
            DataManager.ReLoad();
        }

        GUILayout.BeginHorizontal();

        ShowTypeAndAddNewData();

        GUILayout.EndHorizontal();

        GUILayout.Space(6);

        GameDatas = new List<GameData>(DataManager.GetGameDatas());

        List<GameData> list = new List<GameData>();

        DataNames = GetGameDataNames(GameDatas, ref list);

        if (DataNames != null && DataNames.Length != 0 && list.Count > 0)
        {
            GUILayout.BeginHorizontal();
            SelectScrollView(DataNames);
            GUILayout.BeginHorizontal();

            GameData data = list[SelectIetmIndex = Mathf.Clamp(SelectIetmIndex, 0, list.Count - 1)];
            DisplayGameData(data, fields, InputStrings);

            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();

            if (GUILayout.Button("保存数据"))
            {
                SaveGameData(data, fields, InputStrings);
            }
        }

    }


    private void SaveGameData(GameData data, string[] fields, string[] InputStrings)
    {
        data.Content.Clear();
        data.SaveValue("0", InputStrings[0]);

        for (int i = 1; i < fields.Length; i++)
        {
            if (!string.IsNullOrEmpty(fields[i]))
            {
                data.SaveValue(fields[i], InputStrings[i]);
            }
        }

        DataManager.Save(false);
    }

    private void DisplayGameData(GameData data, string[] fields, string[] InputStrings)
    {
        GUILayout.BeginVertical(GUILayout.Width(80)); //Leable
        LabelColoum(data.Content.Keys);
        GUILayout.EndVertical(); //Leable


        GUILayout.BeginVertical(); //Input Text Field

        int count = data.Content.Count;
        InitFieldInput(data, fields, InputStrings);

        for (int i = 0; i < count; i++)
        {
            bool ispop;
            GameDataType type;
            isPopup(data, fields[i], out ispop, out type);

            if (ispop)
            {
                DisplayPopInput(type, i);
            }
            else
            {
                InputStrings[i] = GUILayout.TextField(InputStrings[i]);
            }
        }

        if (GUILayout.Button("重置", GUILayout.Height(19)))
        {
            ResetInputField();
        }

        GUILayout.EndVertical(); //Input Text Field

    }


    private string[] GetGameDataNames(List<GameData> olist, ref List<GameData> list)
    {
        if (ShowGameDataTypeIndex >= DataManager.TypeFilter.Count)
        {
            list = olist;
        }
        else
        {
            foreach (var item in olist)
            {
                if (item == null) continue;
                if (item.DataType == DataManager.TypeFilter[ShowGameDataTypeIndex])
                {
                    list.Add(item);
                }
            }
        }
        string[] names = new string[list.Count];

        if (list != null)
        {
            names = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                names[i] = list[i].GetString(0);
            }
        }
        return names;
    }

    private string[] GetGameDataNames(List<GameData> olist, GameDataType type)
    {
        string[] names = null;
        if (type == GameDataType.SkillEffect || type == GameDataType.UnitEffect || type == GameDataType.TeamEffect)
        {
            if (type == GameDataType.SkillEffect)
            {
                names = Enum.GetNames(typeof(SkillEffectName));
            }
            else if (type == GameDataType.UnitEffect)
            {
                names = Enum.GetNames(typeof(UnitEffectName));
            }
            else if (type == GameDataType.TeamEffect)
            {
                names = Enum.GetNames(typeof(TeamEffectName));
            }
            return names;
        }

        List<GameData> list = new List<GameData>();
        foreach (var item in olist)
        {
            if (item == null) continue;
            if (item.DataType == type)
            {
                list.Add(item);
            }
        }

        names = new string[list.Count];

        if (list != null)
        {
            names = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                names[i] = list[i].GetString(0);
            }
        }
        return names;
    }

    private void ShowTypeAndAddNewData()
    {
        GUILayout.Label("显示类型：", GUILayout.Width(55));
        string[] GameDataTypeNames = GetDataTypeNames();
        string[] ShowTypeNames = new string[GameDataTypeNames.Length + 1];
        Array.Copy(GameDataTypeNames, ShowTypeNames, GameDataTypeNames.Length);
        ShowTypeNames[ShowTypeNames.Length - 1] = "显示所有";

        ShowGameDataTypeIndex = EditorGUILayout.Popup(ShowGameDataTypeIndex, ShowTypeNames);        //---

        if (LastShowGameDataTypeIndex != ShowGameDataTypeIndex)
        {
            ResetInputField();
            LastShowGameDataTypeIndex = ShowGameDataTypeIndex;
        }

        GUILayout.Label("添加类型：", GUILayout.Width(55));
        SelectGameDataTypeIndex = EditorGUILayout.Popup(SelectGameDataTypeIndex, GameDataTypeNames);

        if (GUILayout.Button("添加数据"))
        {
            GameData data;
            GameDataType type = DataManager.TypeFilter[SelectGameDataTypeIndex];


            data = DataManager.CreateGameData(type, "New" + Enum.GetName(typeof(GameDataType), type));
            DataManager.AddGameData(data);
            /* 
            if (TypeIsStatic(type))
            {
                data = Game.StaticDataManager.CreateGameData(type, "New" + Enum.GetName(typeof(GameDataType), type));
                Game.StaticDataManager.AddGameData(data);
            }
            else
            {
                data = Game.DynamicDataManager.CreateGameData(type, "New" + Enum.GetName(typeof(GameDataType), type));
                Game.DynamicDataManager.AddGameData(data);
            }*/
            ShowGameDataTypeIndex = SelectGameDataTypeIndex;
            SelectIetmIndex = int.MaxValue;
            ResetInputField();
        }
    }

    private string[] GetDataTypeNames()
    {
        string[] GameDataTypeNames = new string[DataManager.TypeFilter.Count];
        for (int i = 0; i < DataManager.TypeFilter.Count; i++)
        {
            GameDataTypeNames[i] = Enum.GetName(typeof(GameDataType), DataManager.TypeFilter[i]);
        }

        return GameDataTypeNames;
    }

    public void SelectScrollView(string[] DataNames)
    {

        GUIStyle textStyle = new GUIStyle("textfield");
        GUIStyle buttonStyle = new GUIStyle("button");
        textStyle.active = buttonStyle.active;
        textStyle.onNormal = buttonStyle.onNormal;

        ScrollViewV2 = GUILayout.BeginScrollView(ScrollViewV2, true, true, GUILayout.Width(155), GUILayout.Height(595));
        {
            SelectIetmIndex = GUILayout.SelectionGrid(SelectIetmIndex, DataNames, 1, textStyle);
        }

        GUILayout.EndScrollView();
    }

    public void LabelColoum(IEnumerable keys)
    {
        GUILayout.Label("Name", TextCenterStyle, GUILayout.Height(18));
        int i = 0;
        foreach (var item in keys)
        {
            i++;
            if (i == 1) continue;
            GUILayout.Label((string)item, TextCenterStyle, GUILayout.Height(18));
        }
    }

    public void InitFieldInput(GameData data, string[] fields, string[] InputStrings)
    {
        if (LastSelectIetmIndex != SelectIetmIndex)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = "";
                InputStrings[i] = "";
            }

            InputStrings[0] = data.Content["0"];
            LastSelectIetmIndex = SelectIetmIndex;

            int index = 0;
            foreach (var item in data.Content.Keys)
            {
                InputStrings[index] = data.Content[item];
                fields[index] = item;
                index++;
            }

            PopupSelect = new int[30];
        }
    }

    public void ResetInputField()
    {
        LastSelectIetmIndex = -1;
    }

    int[] PopupSelect = new int[30];
    private void DisplayPopInput(GameDataType type, int index)
    {
        string[] s_names = GetGameDataNames(DynamicAndStaicGameDatas, type);
        string[] names = new string[s_names.Length + 1];
        names[0] = "无";
        s_names.CopyTo(names, 1);


        Array array = null;

        //
        if (type == GameDataType.SkillEffect)
        {
            array = Enum.GetValues(typeof(SkillEffectName));
        }
        else if (type == GameDataType.UnitEffect)
        {
            array = Enum.GetValues(typeof(UnitEffectName));
        }
        else if (type == GameDataType.TeamEffect)
        {
            array = Enum.GetValues(typeof(TeamEffectName));
        }
        //


        if (array == null)
        {
            for (int i = 0; i < names.Length; i++)
            {

                if (InputStrings[index] == names[i])
                    PopupSelect[index] = i;
            }
        }
        else
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (InputStrings[index] == "" + (int)array.GetValue(i))
                    PopupSelect[index] = i + 1;
            }
        }

        PopupSelect[index] = EditorGUILayout.Popup(PopupSelect[index], names);
        if (PopupSelect[index] == 0)
            InputStrings[index] = "";
        else
        {
            if (array != null)
            {
                InputStrings[index] = "" + (int)array.GetValue(PopupSelect[index] - 1);
            }
            else
            {
                InputStrings[index] = names[PopupSelect[index]];
            }
        }
    }

    private void isPopup(GameData data, string v, out bool ispop, out GameDataType type)
    {
        if (v.StartsWith("Buff") && data.DataType == GameDataType.UnitSetting)
        {
            ispop = true;
            type = GameDataType.UnitBuff;
            return;
        }
        if (v.StartsWith("CityBuff") && data.DataType == GameDataType.City)
        {
            ispop = true;
            type = GameDataType.TeamBuff;
            return;
        }
        if (v.StartsWith("Buff") && data.DataType == GameDataType.Hero)
        {
            ispop = true;
            type = GameDataType.TeamBuff;
            return;
        }
        if (v.StartsWith("Skill"))
        {
            ispop = true;
            type = GameDataType.Skill;
            return;
        }
        if (v.StartsWith("Army"))
        {
            ispop = true;
            type = GameDataType.Army;
            return;
        }
        if (v.StartsWith("King"))
        {
            ispop = true;
            type = GameDataType.King;
            return;
        }
        if (v.StartsWith("Team") || v.StartsWith("ShopTeam"))
        {
            ispop = true;
            type = GameDataType.Team;
            return;
        }
        if (v.StartsWith("MainHero") || v.StartsWith("MinorHero"))
        {
            ispop = true;
            type = GameDataType.Hero;
            return;
        }
        if (v.StartsWith("Unit"))
        {
            ispop = true;
            type = GameDataType.Unit;
            return;
        }
        if (v.StartsWith("Setting"))
        {
            ispop = true;
            type = GameDataType.UnitSetting;
            return;
        }
        if (v.StartsWith("EffectIDs") && data.DataType == GameDataType.Skill)
        {
            ispop = true;
            type = GameDataType.SkillEffect;
            return;
        }
        if (v.StartsWith("EffectIDs") && data.DataType == GameDataType.TeamBuff)
        {
            ispop = true;
            type = GameDataType.TeamEffect;
            return;
        }
        if (v.StartsWith("EffectIDs") && data.DataType == GameDataType.UnitBuff)
        {
            ispop = true;
            type = GameDataType.UnitEffect;
            return;
        }

        if (v.StartsWith("NextComm_"))
        {
            ispop = true;
            type = GameDataType.ConversationData;
            return;
        }

        if (v.StartsWith("CommunicationEntry") || v.StartsWith("CommEntry"))
        {
            ispop = true;
            type = GameDataType.ConversationData;
            return;
        }

        ispop = false; type = GameDataType.Other;
    }

    bool TypeIsStatic(GameDataType type)
    {
        switch (type)
        {
            case GameDataType.King:
            case GameDataType.Army:
            case GameDataType.Team:
            case GameDataType.Hero:
            case GameDataType.Unit:
            case GameDataType.Skill:
            case GameDataType.TeamBuff:
            case GameDataType.UnitBuff:
            case GameDataType.Other:
            case GameDataType.City:
                return false;
            case GameDataType.UnitSetting:
            case GameDataType.UnitEffect:
            case GameDataType.TeamEffect:
            case GameDataType.SkillEffect:
            case GameDataType.ConversationData:
                return true;
            default: throw new Exception("Type Not Found");
        }
    }
}
