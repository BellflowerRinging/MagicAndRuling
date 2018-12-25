using System.Collections;
using System.Collections.Generic;

public class King : GameData
{
    public Army army { get; private set; }
    public CommunicationData CommunicationEntry { get; set; }


    public Skill[] Skills = new Skill[3];
    public int Money;
    public string MapUnitPostion;

    public King(string name, bool UserSoure = true) : base(GameDataType.King, name)
    {
        Init(UserSoure);
    }

    public King(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];
        army = InitParaByDynamic<Army>(UserSoure, "Army", "");
        CommunicationEntry = InitParaByStatic<CommunicationData>(UserSoure, "CommunicationEntry", "");
        if (CommunicationEntry == null) CommunicationEntry = Game.StaticDataManager.GetGameData<CommunicationData>("DefautTalk");
        Money = InitIntValue("Money");
        MapUnitPostion = InitStringValue("MapPostion");
        if (army != null) { army.SetKing(this); }
        Skills = InitParas<Skill>(UserSoure, "Skill", Skills.Length, Game.DynamicDataManager);
    }

    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SavePara("Army", army);
        SaveParas("Skill", Skills);
        SaveValue("Money", Money);
        SaveValue("MapPostion", MapUnitPostion);
        SavePara("CommunicationEntry", CommunicationEntry);
    }

    public override List<GameData> GetChildrens()
    {
        List<GameData> list = new List<GameData>();
        list.Add(army);
        for (int i = 0; i < Skills.Length; i++)
        {
            list.Add(Skills[i]);
        }
        return list;
    }

    public void SetArmy(Army army)
    {
        if (army != null)
            army.SetKing(this);
        this.army = army;
        SaveValue("Army", army.Name);
    }



    //战斗力占比


    public int Power
    {
        get
        {
            int sum = 0;
            foreach (var item in this.army.team)
            {
                if (item != null)
                {
                    sum += item.Power;
                }
            }
            Game.EventTextContorl.AddEventTextLn("All:" + sum);
            return sum;
        }
    }

}