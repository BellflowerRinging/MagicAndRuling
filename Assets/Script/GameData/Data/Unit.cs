using System.Collections;
using System.Collections.Generic;

public class Unit : GameData
{
    public King king;
    public Team team;
    public UnitSetting Setting;
    public int Count;
    public int Injury;
    public int Morale;

    public Unit(string name, UnitSetting Setting = null, bool UserSoure = true) : base(GameDataType.Unit, name)
    {
        this.Setting = Setting;
        Init(UserSoure);
    }
    public Unit(GameData data, bool init = true, bool UserSoure = true) : base(data)
    {
        if (init)
            Init(UserSoure); //
    }
    ///
    //public Unit(Unit unit)
    //{

    //}
    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];

        if (Setting == null)
            // if (Content.ContainsKey("Setting"))
            //     Setting = GameDataTool.GetOrNewGameDataByStatic<UnitSetting>(false, Content["Setting"]);
            // else SaveValue("Setting", "");
            Setting = InitParaByStatic<UnitSetting>(false, "Setting");
        else SaveValue("Setting", Setting.Name);

        //if (Content.ContainsKey("King") && UserSoure) { this.king = Game.DynamicDataManager.GetGameData<King>(Content["King"]); } else { SaveValue("King", ""); }
        if (UserSoure) king = InitParaByDynamic<King>(UserSoure, "king");

        //if (Content.ContainsKey("Team") && UserSoure) { this.team = Game.DynamicDataManager.GetGameData<Team>(Content["Team"]); } else { SaveValue("Team", ""); }
        if (UserSoure) team = InitParaByDynamic<Team>(UserSoure, "Team");


        if (king != null && team != null)
            if (team.army.king != king) throw new System.Exception("team.army.king != king");

        //       if (this.king == null && this.team != null)
        //         this.king = this.team.army.king;

        // if (Content.ContainsKey("Count")) { Count = int.Parse(Content["Count"]); } else { SaveValue("Count", "0"); };
        // if (Content.ContainsKey("Injury")) { Injury = int.Parse(Content["Injury"]); } else { SaveValue("Injury", "0"); };
        // if (Content.ContainsKey("Morale")) { Morale = int.Parse(Content["Morale"]); } else { SaveValue("Morale", "0"); };
        Count = InitIntValue("Count");
        Injury = InitIntValue("Injury");
        Morale = InitIntValue("Morale");
    }

    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SavePara("King", king);
        SavePara("Team", team);
        SavePara("Setting", Setting);
        SaveValue("Count", Count + "");
        SaveValue("Injury", Injury + "");
        SaveValue("Morale", Morale + "");
        // if (king != null) SaveValue("King", king.Name);
        // if (team != null) SaveValue("Team", team.Name);
    }

    public UnitSummaryData GetSummary()
    {
        //int maxhp = 0, hp = 0, ad = 0, ed = 0, count = 0, injury = 0, Morale = 0;
        UnitSummaryData dic = new UnitSummaryData();
        dic.name = Setting.Name;
        dic.Maxhp = Count * Setting.HP;
        dic.Injury = Injury;
        dic.Morale = Morale;
        dic.AllCount = Count;
        dic.Count = dic.AllCount - Injury;
        dic.Hp = dic.Count * Setting.HP;
        dic.Ad = dic.Count * Setting.AD;
        dic.Ed = dic.Count * Setting.DE;
        return dic;
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        if (team != null && team.army != null)
            king = team.army.king;
    }

    public int GetBattleCount()
    {
        return Count - Injury;
    }

    public void DoBuff(BattleUnitData data, BattleUnitData todata, BattleData battle, EffectTimeType time)
    {
        foreach (var buff in Setting.buff)
        {
            if (buff == null) continue;
            buff.DoBuff(data, todata, battle, time);
        }
    }

    public void DoBuff(EffectTimeType time)
    {
        foreach (var buff in Setting.buff)
        {
            if (buff == null) continue;
            buff.DoBuff(this, time);
        }
    }

    public int Power
    {
        get
        {
            int unit = (int)((Count - Injury) * (Morale * PowerConst.V4 + Setting.Power) * PowerConst.V6);
            int s = ((Count - Injury) * PowerConst.V + unit);
            //Game.EventTextContorl.AddEventTextLn(string.Format("Count:{0} Mor:{1} Unit:{2}", Count * PowerConst.V, Morale * PowerConst.V4, unit));
            return s;
        }
    }
}

public class UnitSummaryData
{
    public int Maxhp;
    public int Hp;
    public int Ad;
    public int Ed;
    public int AllCount;
    public int Count;
    public int Injury;
    public int Morale;
    public string name;
}