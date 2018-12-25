using System.Collections;
using System.Collections.Generic;

//GamaData无法对List处理！！

//静态数据并不合理

public class GameDataTool
{

    //UserSoure 是否使用源数据 否则复制数据 默认为否
    public static T GetOrNewGameDataByDynamic<T>(bool UserSoure, string name) where T : GameData
    {
        if (string.IsNullOrEmpty(name)) return null;
        T t = Game.DynamicDataManager.GetGameData<T>(name);
        if (!UserSoure)
            t = (T)Game.DynamicDataManager.CreateGameData(t, true, UserSoure);
        return t;
    }

    public static T GetOrNewGameDataByStatic<T>(bool UserSoure, string name) where T : GameData
    {
        if (string.IsNullOrEmpty(name)) return null;
        T t = Game.StaticDataManager.GetGameData<T>(name);
        if (!UserSoure)
            t = (T)Game.StaticDataManager.CreateGameData(t, true, UserSoure);
        return t;
    }

    public static T GetOrNewGameData<T>(bool UserSoure, string name, DataManager manager) where T : GameData
    {
        if (manager == Game.DynamicDataManager) return GetOrNewGameDataByDynamic<T>(UserSoure, name);
        if (manager == Game.StaticDataManager) return GetOrNewGameDataByStatic<T>(UserSoure, name);

        if (string.IsNullOrEmpty(name)) return null;
        T t = manager.GetGameData<T>(name);
        if (!UserSoure)
            t = (T)manager.CreateGameData(t, true, UserSoure);
        return t;
    }
    
}


