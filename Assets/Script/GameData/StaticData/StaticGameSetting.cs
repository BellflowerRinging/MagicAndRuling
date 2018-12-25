using System.Collections;
using System.Collections.Generic;

public class StaticGameSetting : GameData
{
    public StaticGameSetting(string name, bool UserSoure = true) : base(GameDataType.StaticGameSetting, name)
    {
        Init(UserSoure);
    }

    public StaticGameSetting(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];
    }

    public override void SaveToContent()
    {
        Name = Content["0"];
    }

    /*public override List<GameData> GetChildrens()
    {

    }*/

}