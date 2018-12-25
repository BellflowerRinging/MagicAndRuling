using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : IEquatable<GameData>
{
    public string Name;
    public GameDataType DataType;
    public Dictionary<string, string> Content;

    public GameData()
    {

    }

    public GameData(GameDataType type, string name)
    {
        DataType = type;
        Content = new Dictionary<string, string>();
        Content.Add("0", name);
        Name = name;
    }

    public GameData(GameData gamedata)
    {
        if (gamedata == null)
            throw new Exception("gamedata==null");
        if (gamedata.Content == null)
            throw new Exception("gamedata.Content==null");
        DataType = gamedata.DataType;
        Content = new Dictionary<string, string>(gamedata.Content);
        Name = gamedata.Name;
    }
    public virtual void Init(bool UserSoure)
    {
        Name = Content["0"];
    }
    public virtual string GetString(int index)
    {
        if (Content.ContainsKey(index + ""))
            return Content[index + ""];
        else
            throw new Exception("This Key " + index + " Not Found");
    }

    /*public string[] GetFieldNames()
    {
        switch (DataType)
        {
            case GameDataType.King: return Enum.GetNames(typeof(KingFieldType));
            case GameDataType.Army: return Enum.GetNames(typeof(ArmyFieldType));
            case GameDataType.Team: return Enum.GetNames(typeof(TeamFieldType));
            case GameDataType.Hero: return Enum.GetNames(typeof(HeroFieldType));
            case GameDataType.City: return Enum.GetNames(typeof(CityFieldType));
            case GameDataType.Other: return null;
            default: return null;
        }
    }*/

    public void SaveValue(string key, string value)
    {
        if (!Content.ContainsKey(key))
        {
            Content.Add(key, value);
        }
        else
        {
            Content[key] = value;
        }
    }

    public void SaveValue(string key, object obj)
    {
        SaveValue(key, obj.ToString());
    }

    public void SaveValue(int key, string value)
    {
        SaveValue(key + "", value);
    }

    public void SavePara(string key, GameData data)
    {
        if (data != null) SaveValue(key, data.Name);
    }

    public void SaveParas(string key, GameData[] datas)
    {
        for (int i = 0; i < datas.Length; i++)
        {
            SavePara(key + "s_" + i, datas[i]);
        }
    }

    public void DelValue(string key)
    {
        Content.Remove(key);
    }

    public void DelValue(int index)
    {
        string dk = null;
        int i = 0;
        foreach (var item in Content.Keys)
        {
            if (i == index)
            {
                dk = item;
                break;
            }
            i++;
        }
        if (dk != null)
            DelValue(dk);
    }

    public virtual void SaveToContent()
    {
        SaveValue("0", Name);
    }

    public virtual List<GameData> GetChildrens() { return null; }
    public T InitParaByDynamic<T>(bool UserSoure, string name, string init = "") where T : GameData
    {
        T t = default(T);
        if (Content.ContainsKey(name))
            t = GameDataTool.GetOrNewGameDataByDynamic<T>(UserSoure, Content[name]);
        else { SaveValue(name, init); }
        return t;
    }

    public T InitParaByStatic<T>(bool UserSoure, string name, string init = "") where T : GameData
    {
        T t = default(T);
        if (Content.ContainsKey(name))
            t = GameDataTool.GetOrNewGameDataByStatic<T>(UserSoure, Content[name]);
        else { SaveValue(name, init); }
        return t;
    }

    public T InitPara<T>(bool UserSoure, string name, DataManager manager, string init = "") where T : GameData
    {
        T t = default(T);
        if (Content.ContainsKey(name))
            t = GameDataTool.GetOrNewGameData<T>(UserSoure, Content[name], manager);
        else { SaveValue(name, init); }
        return t;
    }

    public T[] InitParas<T>(bool UserSoure, string name, int length, DataManager manager, string init = "") where T : GameData
    {
        T[] t = new T[length];
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = InitPara<T>(UserSoure, name + "s_" + i, manager, init);
        }
        return t;
    }

    public int InitIntValue(string name, int init = 0)
    {
        if (Content.ContainsKey(name))
            if (!string.IsNullOrEmpty(Content[name]))
                return int.Parse(Content[name]);
        SaveValue(name, init + "");
        return init;
    }

    public long InitLongValue(string name, long init = 0)
    {
        if (Content.ContainsKey(name))
            if (!string.IsNullOrEmpty(Content[name]))
                return long.Parse(Content[name]);
        SaveValue(name, init + "");
        return init;
    }

    public string InitStringValue(string name, string init = "")
    {
        if (Content.ContainsKey(name)) return Content[name];
        else SaveValue(name, init);
        return init;
    }

    public float InitFloatValue(string name, float init = 0f)
    {
        if (Content.ContainsKey(name))
            if (!string.IsNullOrEmpty(Content[name]))
                return float.Parse(Content[name]);
        SaveValue(name, init + "");
        return init;
    }

    public bool InitBooltValue(string name, bool init = false)
    {
        if (Content.ContainsKey(name))
            if (Content[name] == "0") return false;
            else return true;
        else
            if (init) SaveValue(name, "1"); else SaveValue(name, "0");
        return init;
    }














    public override bool Equals(object obj)
    {
        return Equals(obj as GameData);
    }

    public bool Equals(GameData other)
    {
        if (other == null) return false;

        if (Content != null && other.Content != null)
        {
            if (Content.ContainsKey("0") && other.Content.ContainsKey("0"))
            {
                if (Content["0"].Equals(other.Content["0"]))
                {
                    return true;
                }
            }
        }

        return other != null &&
               DataType == other.DataType &&
               EqualityComparer<Dictionary<string, string>>.Default.Equals(Content, other.Content);
    }

    public override int GetHashCode()
    {
        var hashCode = -1648415273;
        hashCode = hashCode * -1521134295 + DataType.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(Content);
        return hashCode;
    }

    public static bool operator ==(GameData data1, GameData data2)
    {
        return EqualityComparer<GameData>.Default.Equals(data1, data2);
    }

    public static bool operator !=(GameData data1, GameData data2)
    {
        return !(data1 == data2);
    }



}


