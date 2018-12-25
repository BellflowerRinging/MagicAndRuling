using System.Collections.Generic;

public class City : GameData
{
    public string CityName;
    public CommunicationData[] CommunicationEntrys = new CommunicationData[6];
    public Team ShopTeam { get; set; }
    public TeamBuff[] buff = new TeamBuff[5];
    public string Introduce { get; set; }
    public string BuffIntroduce { get; set; }

    public City(string name, bool UserSoure = true) : base(GameDataType.City, name)
    {
        Init(UserSoure);
    }

    public City(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure)
    {
        Name = Content["0"];
        buff = InitParas<TeamBuff>(UserSoure, "CityBuff", buff.Length, Game.DynamicDataManager);
        CityName = InitStringValue("CityName");
        Introduce = InitStringValue("Intorduce");
        BuffIntroduce = InitStringValue("BuffIntorduce");
        CommunicationEntrys = InitParas<CommunicationData>(UserSoure, "CommEntry", CommunicationEntrys.Length, Game.StaticDataManager);
        ShopTeam = InitParaByDynamic<Team>(UserSoure, "ShopTeam");
    }


    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SaveParas("CityBuff", buff);
        SaveValue("CityName", CityName);
        SaveValue("Introduce", Introduce);
        SaveValue("BuffIntroduce", BuffIntroduce);
        SaveParas("CommEntry", CommunicationEntrys);
        SavePara("ShopTeam", ShopTeam);
    }
}