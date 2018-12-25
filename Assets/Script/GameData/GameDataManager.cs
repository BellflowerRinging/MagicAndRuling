using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum GameDataType
{
    King = 0,
    Army = 1,
    Team = 2,
    Hero = 3,
    Unit = 4,
    UnitSetting = 6,
    Skill = 5,
    TeamBuff = 7,
    UnitBuff = 8,
    City = 9,
    Other = 1000,
    DynamicGameSetting = 1001,
    StaticGameSetting = 1002,
    UnitEffect = 20,
    TeamEffect = 21,
    SkillEffect = 22,
    ConversationData = 23,
}

public class DataManager
{
    public string FliePart;
    protected string LoadName;

    protected List<GameData> GameDatas;
    public List<GameDataType> TypeFilter;

    public readonly bool IsStatic;

    private DataManager() { }

    public DataManager(string LoadName, bool IsStatic, List<GameDataType> Filter = null)
    {
        this.LoadName = LoadName;
        FliePart = Application.dataPath + "/Resources/" + LoadName + ".json";
        GameDatas = new List<GameData>();
        if (Filter != null) TypeFilter = new List<GameDataType>(Filter);
        this.IsStatic = IsStatic;
    }

    public void Load()
    {
        GameDatas = new List<GameData>();

        UnityEngine.Object obj = Resources.Load(LoadName);
        string str = obj == null ? "" : obj.ToString();

        if (!string.IsNullOrEmpty(str))
        {
            GameData data;
            List<GameData> SourceGameDatas = JsonConvert.DeserializeObject<List<GameData>>(str);

            foreach (var item in SourceGameDatas)
            {
                if (TypeFilter != null)
                {
                    if (!TypeFilter.Contains(item.DataType))
                    {
                        continue;
                    }
                }
                data = CreateGameData(item, false);
                GameDatas.Add(data);
            }
        }
    }

    public void InitGameData()
    {
        foreach (var item in GameDatas)
        {
            if (item == null) continue;
            item.Init(true);
        }
    }

    public List<GameData> GetGameDatas()
    {
        // if (GameDatas == null) throw new Exception("GameDatas==null");
        if (GameDatas.Count == 0)
            Load();
        return GameDatas;
    }

    public List<T> GetGameDatas<T>(GameDataType type, bool UserSoure = false) where T : GameData
    {
        // if (GameDatas == null) throw new Exception("GameDatas==null");
        if (GameDatas.Count == 0)
            Load();

        List<T> list = new List<T>();

        foreach (var item in GameDatas)
        {
            if (item.DataType == type)
            {
                if (UserSoure)
                {
                    list.Add((T)item);
                }
                else
                {
                    list.Add((T)CreateGameData(item, true, false));
                }
            }
        }

        return list;
    }

    public List<GameData> GetGameDatas(GameDataType type, bool UserSoure = false)
    {
        return GetGameDatas<GameData>(type, UserSoure);
    }

    public GameData GetGameData(string name)
    {
        return GetGameData<GameData>(name);
    }

    public T GetGameData<T>(string name) where T : GameData
    {
        if (string.IsNullOrEmpty(name))
            return null;
        foreach (var item in GameDatas)
        {
            if (item == null) continue;
            if (item.Content.ContainsKey("0") && item.Content["0"] == name)
            {
                return (T)item;
            }
        }
        return null;
    }

    public void ReLoad()
    {
        Load();
    }

    public void Save(bool IsInGame = true)
    {
        if (IsInGame && !IsStatic)
            foreach (var item in GameDatas)
                item.SaveToContent();

        List<GameData> datas = new List<GameData>();
        foreach (var item in GameDatas)
        {
            //if (IsInGame && item.DataType == GameDataType.UnitSetting) continue; //游戏中 可在此处忽略掉静态数据Content的保存
            if (TypeFilter != null) if (!TypeFilter.Contains(item.DataType)) //过滤掉不小心存进来的GameData
                    continue;

            datas.Add(new GameData(item)); //仅仅只是复制一遍 可能没有意义 
        }

        string str = JsonConvert.SerializeObject(datas);

        ResourcesManager.WriteToJsonFile(FliePart, str);
    }

    public void AddGameDataRange(GameData data)
    {
        if (data == null) return;
        AddGameData(data);
        List<GameData> list = data.GetChildrens();
        if (list != null)
            foreach (var item in list)
            {
                AddGameDataRange(item);
            }
    }

    public void AddGameData(GameData data)
    {
        if (!GameDatas.Contains(data))
        {
            GameDatas.Add(data);
            AddGameDataRange(data);
        }
        //else throw new Exception("GameDatas.Contains(" + data.Content["0"] + ")");
    }

    public GameData CreateGameData(GameDataType type, string name) //这是为方便GameDataEditWindow  未知类型时
    {
        GameData data = null;
        if (string.IsNullOrEmpty(name)) return null;
        switch (type)
        {
            case GameDataType.King: data = new King(name); break;
            case GameDataType.Army: data = new Army(name); break;
            case GameDataType.Team: data = new Team(name); break;
            case GameDataType.Hero: data = new Hero(name); break;
            case GameDataType.TeamBuff: data = new TeamBuff(name); break;
            case GameDataType.UnitBuff: data = new UnitBuff(name); break;
            case GameDataType.Skill: data = new Skill(name); break;
            case GameDataType.City: data = new City(name); break;
            case GameDataType.UnitSetting: data = new UnitSetting(name); break;
            case GameDataType.Unit: data = new Unit(name); break;
            case GameDataType.ConversationData: data = new CommunicationData(name); break;
            case GameDataType.Other: data = new GameData(GameDataType.Other, name); break;
            case GameDataType.DynamicGameSetting: data = new DynamicGameSetting(name); break;
            case GameDataType.StaticGameSetting: data = new StaticGameSetting(name); break;
            default: break;
        }
        return data;
    }

    public GameData CreateGameData(GameData item, bool init = true, bool UserSoure = true) //复制  未知类型时
    {
        GameData data = null;
        if (item == null) return null;
        switch (item.DataType)
        {
            case GameDataType.King: data = new King(item, init, UserSoure); break;
            case GameDataType.Army: data = new Army(item, init, UserSoure); break;
            case GameDataType.Team: data = new Team(item, init, UserSoure); break;
            case GameDataType.Hero: data = new Hero(item, init, UserSoure); break;
            case GameDataType.TeamBuff: data = new TeamBuff(item, init, UserSoure); break;
            case GameDataType.UnitBuff: data = new UnitBuff(item, init, UserSoure); break;
            case GameDataType.Skill: data = new Skill(item, init, UserSoure); break;
            case GameDataType.City: data = new City(item, init, UserSoure); break;
            case GameDataType.UnitSetting: data = new UnitSetting(item, init, UserSoure); break;
            case GameDataType.Unit: data = new Unit(item, init, UserSoure); break;
            case GameDataType.ConversationData: data = new CommunicationData(item, init, UserSoure); break;
            case GameDataType.Other: data = new GameData(item); break;
            case GameDataType.DynamicGameSetting: data = new DynamicGameSetting(item); break;
            case GameDataType.StaticGameSetting: data = new StaticGameSetting(item); break;
            default: break;
        }
        return data;
    }

    public void DelGameData(GameData data)
    {
        if (data != null)
            GameDatas.Remove(data);
    }

    public void DelGameData(string name)
    {
        DelGameData(GetGameData(name));
    }

    public void UpdataToContent()
    {
        foreach (var item in GameDatas)
        {
            item.SaveToContent();
        }
    }

}


