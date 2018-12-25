using System.Collections;
using System.Collections.Generic;


public class Team : GameData
{
    public Army army;
    public Hero MainHero { get; private set; }
    public Hero MinorHero { get; private set; }
    public Unit[] units = new Unit[20]; //GamaData无法对List处理！！  //暂定一个Team里面最多20种兵

    public Team(string name, bool UserSoure = true) : base(GameDataType.Team, name)
    {
        Init(UserSoure);
    }
    public Team(GameData data, bool init = true, bool UserSoure = true) : base(data)
    {
        if (init)
            Init(UserSoure);
    }
    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];

        //if (Content.ContainsKey("Army") && UserSoure) { this.army = Game.DynamicDataManager.GetGameData<Army>(Content["Army"]); } else { SaveValue("Army", ""); }
        if (UserSoure) army = InitParaByDynamic<Army>(UserSoure, "Army", "");

        // if (Content.ContainsKey("MainHero"))
        //     MainHero = GameDataTool.GetOrNewGameDataByDynamic<Hero>(UserSoure, Content["MainHero"]);
        // else { SaveValue("MainHero", ""); }
        MainHero = InitParaByDynamic<Hero>(UserSoure, "MainHero");

        if (MainHero != null) MainHero.SetTeam(this);

        // if (Content.ContainsKey("MinorHero"))
        //     MinorHero = GameDataTool.GetOrNewGameDataByDynamic<Hero>(UserSoure, Content["MinorHero"]);
        // else { SaveValue("MinorHero", ""); }
        MinorHero = InitParaByDynamic<Hero>(UserSoure, "MinorHero");
        if (MinorHero != null) MinorHero.SetTeam(this);

        // for (int i = 0; i < units.Length; i++)
        // {
        //     if (Content.ContainsKey("Unit_" + i))
        //         units[i] = GameDataTool.GetOrNewGameDataByDynamic<Unit>(UserSoure, Content["Unit_" + i]);
        //     else { SaveValue("Unit_" + i, ""); }
        // }
        units = InitParas<Unit>(UserSoure, "Unit", units.Length, Game.DynamicDataManager);


        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] != null)
            {
                units[i].SetTeam(this);
            }
        }
    }
    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SavePara("Army", army);
        // if (MainHero != null) SaveValue("MainHero", MainHero.Name); else { SaveValue("MainHero", ""); }
        // if (MinorHero != null) SaveValue("MinorHero", MinorHero.Name); else { SaveValue("MinorHero", ""); }
        SavePara("MainHero", MainHero);
        SavePara("MinorHero", MinorHero);
        SaveParas("Unit", units);
        // for (int i = 0; i < units.Length; i++)
        // {
        //     if (units[i] != null) SaveValue("Unit_" + i, units[i].Name);
        // }
    }

    public override List<GameData> GetChildrens()
    {
        List<GameData> list = new List<GameData>();
        list.Add(MainHero);
        list.Add(MinorHero);
        for (int i = 0; i < units.Length; i++)
        {
            list.Add(units[i]);
        }
        return list;
    }

    public void AddUnit(Unit unit, bool toScore = false)
    {
        if (unit == null) return;

        foreach (var item in units) //默认同种单位不重复
        {
            if (item == null) continue;
            if (item.Setting.Name == unit.Setting.Name)
            {
                item.Count += unit.Count;
                item.Injury += unit.Injury;
                item.Morale += unit.Morale;
                return;
            }
        }

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] != null) continue;
            unit.Name = Name + "_Units_" + i;
            unit.SaveToContent();
            units[i] = unit;
            units[i].SetTeam(this);
            if (toScore) Game.DynamicDataManager.AddGameData(unit);
            SaveValue("Units_" + i, units[i].Name);
            return;
        }
    }

    public void RemoveUnit(Unit unit, bool toScore = false)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == null) continue;
            if (unit.Setting.Name == units[i].Setting.Name)
            {
                if (toScore)
                    Game.DynamicDataManager.DelGameData(unit);
                units[i] = null;
                return;
            }
        }
    }

    public void RemoveAllUnit(bool toScore = false)
    {
        if (toScore)
        {
            foreach (var item in units)
            {
                RemoveUnit(item, toScore);
            }
        }
        units = new Unit[units.Length];
    }

    public void SetMainHero(Hero hero)
    {
        if (hero != null)
            hero.SetTeam(this);
        this.MainHero = hero;
        if (MainHero != null) SaveValue("MainHero", MainHero.Name); else { SaveValue("MainHero", ""); }

    }
    public void SetMinorHero(Hero hero)
    {
        if (hero != null)
            hero.SetTeam(this);
        this.MinorHero = hero;
        if (MinorHero != null) SaveValue("MinorHero", MinorHero.Name); else { SaveValue("MinorHero", ""); }
    }

    public TeamSummaryData GetSummary()
    {
        //int maxhp = 0, hp = 0, ad = 0, ed = 0, count = 0, injury = 0, Morale = 0;
        TeamSummaryData data = new TeamSummaryData();
        data.Maxhp = 0;
        data.Hp = 0;
        data.Ad = 0;
        data.Ed = 0;
        data.Count = 0;
        data.AllCount = 0;
        data.Injury = 0;
        data.Morale = 0;
        data.UnitCount = 0;
        foreach (var unit in units)
        {
            if (unit == null)
                continue;
            UnitSummaryData ddic = unit.GetSummary();
            data.Maxhp += ddic.Maxhp;
            data.Hp += ddic.Hp;
            data.Ad += ddic.Ad;
            data.Ed += ddic.Ed;
            data.Count += ddic.Count;
            data.AllCount = ddic.AllCount;
            data.Injury += ddic.Injury;
            data.Morale += ddic.Morale;
            data.UnitCount++;
        }
        /*. (hp ; 0)
            str = string.Format("HP:{0}/{1}\t士气:{2}\n" + "攻击:{3}\t防御:{4}\n" + "人数:{5}\t受伤:{6}",
                         hp, maxhp, Morale, ad, ed, count, injury);*/
        return data;
    }



    public int Power
    {
        get
        {
            TeamSummaryData data = GetSummary();
            int sum = 0;
            foreach (var item in units)
            {
                if (item != null)
                    sum += (item.Power + data.UnitCount * PowerConst.V5);
            }
            sum = (int)(sum * PowerConst.V7);
            //Game.EventTextContorl.AddEventTextLn(string.Format("UnitCount:{0} Sum:{1}\n", data.UnitCount * PowerConst.V5, sum));
            return sum;
        }
    }


    public List<UnitSummaryData> GetAllUnitSummaryData()
    {
        List<UnitSummaryData> datas = new List<UnitSummaryData>();
        foreach (var unit in units)
        {
            if (unit == null) continue;
            datas.Add(unit.GetSummary());
        }
        return datas;
    }

    public int GetBattleCount()
    {
        int Count = 0;
        foreach (var item in units)
        {
            if (item != null)
                Count += item.GetBattleCount();
        }
        return Count;
    }

    public void DoMainHeroBuff(King toKing, TeamAttackingData data, EffectTimeType time)
    {
        if (MainHero == null) return;
        MainHero.DoBuff(toKing, data, time);
    }
    public void DoMinorHeroBuff(King toKing, TeamAttackingData data, EffectTimeType time)
    {
        if (MinorHero == null) return;
        //此处效果减半
        foreach (var item in MinorHero.buff)
        {
            if (item != null) item.HalveEffect();
        }
        MinorHero.DoBuff(toKing, data, time);
    }

    public bool ContainsUnit(Unit unit)
    {
        foreach (var item in units)
        {
            if (item == null) continue;
            if (item.Setting.Name == unit.Setting.Name) return true;
        }
        return false;
    }

    public Unit GetUnit(string SettingName)
    {
        foreach (var item in units)
        {
            if (item == null) continue;
            if (item.Setting.Name == SettingName) return item;
        }
        return null;
    }

}

public class TeamSummaryData
{
    public int Maxhp;
    public int Hp;
    public int Ad;
    public int Ed;
    public int Count;
    public int AllCount;
    public int Injury;
    public int Morale;
    public int UnitCount;
    public string name;

}