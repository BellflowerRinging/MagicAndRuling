using System.Collections;
using System.Collections.Generic;

public class DynamicGameSetting : GameData
{
    public int Time { get; set; }
    public DynamicGameSetting(string name, bool UserSoure = true) : base(GameDataType.DynamicGameSetting, name)
    {
        Init(UserSoure);
    }

    public DynamicGameSetting(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];
        Time = InitIntValue("Time");


    }

    public override void SaveToContent()
    {
        Name = Content["0"];
        SaveValue("Time", Time);
    }

    /*public override List<GameData> GetChildrens()
    {

    }*/

}