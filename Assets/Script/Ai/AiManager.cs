using System;
using System.Collections.Generic;
using UnityEngine;

public class AiManager
{

    public AiManager()
    {

    }
    public King CreateAiKing()
    {
        return CreateAiKing(Game.MapContorl.PlayerUnit.transform.localPosition);
    }

    public King CreateAiKingRangeMapPostion()
    {
        Vector2 mapSize = Game.MapContorl.MapSize;
        float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2);
        float y = UnityEngine.Random.Range(-mapSize.y / 2, mapSize.y / 2);
        Vector2 pos = new Vector2(x, y);
        //Game.EventTextContorl.AddEventTextLn(pos + "");
        return CreateAiKing(pos);
        //return null;
    }

    public King CreateAiKing(Vector2 MapPostion)
    {
        int[] team_power = new int[2];
        int PlayerPower = (int)(Game.Player.Power / PowerConst.V7);
        int pc = UnityEngine.Random.Range(50, 100);
        for (int i = 0; i < team_power.Length; i++) //预选随机战斗力
        {
            team_power[i] = UnityEngine.Random.Range((int)(PlayerPower / 2) - pc, (int)(PlayerPower / 2) + pc);
        }

        King king = new King(RandName(), false);
        Army army = new Army(king.Name + "_Army", false);
        Team[] teams = new Team[3];

        for (int i = 0; i < teams.Length; i++)
        {
            teams[i] = new Team(army.Name + "_Team_" + i, false);
        }

        List<UnitSetting> settings = Game.StaticDataManager.GetGameDatas<UnitSetting>(GameDataType.UnitSetting, false);

        List<UnitSetting> team_0_setting = new List<UnitSetting>(); //按攻击距离筛选
        List<UnitSetting> team_1_setting = new List<UnitSetting>();
        foreach (var item in settings)
        {
            if (item.AttackRange == 1) team_0_setting.Add(item);
            else team_1_setting.Add(item);
        }

        RangeUnitToTeam(team_power[0], teams[0], team_0_setting);
        RangeUnitToTeam(team_power[1], teams[1], team_1_setting);

        army.SetTeam(0, teams[0]);
        army.SetTeam(1, teams[1]);
        army.SetTeam(2, teams[2]);
        king.SetArmy(army);

        //Debug.Log(king.Name + ":" + king.Power);
        Game.DynamicDataManager.AddGameData(king);

        //king.MapUnitPostion = Game.Player.MapUnitPostion;
        king.MapUnitPostion = MapPostion.ToString();
        MapUnit unit = Game.MapContorl.CreateKingMapUnit(king, true); //更新MapContol后用瞬移初始化位置
        //unit.gameObject.transform.localPosition = MapPostion;
        return king;
    }

    private static void RangeUnitToTeam(int team_power, Team team, List<UnitSetting> team_setting)
    {
        int rucount = UnityEngine.Random.Range(1, 4); //1-3种
        List<UnitSetting> rsetting = new List<UnitSetting>();
        for (int i = 0; i < rucount; i++) //从攻击范围合适的Setting中取随机单位种类 
        {
            rsetting.Add(team_setting[UnityEngine.Random.Range(0, team_setting.Count)]);
        }

        int ppc = team_power / rsetting.Count / 2; //+-50%
        foreach (var item in rsetting)
        {
            int power = UnityEngine.Random.Range((int)(team_power / rsetting.Count) - ppc, (int)(team_power / rsetting.Count) + ppc);
            int count = (int)(power / (PowerConst.V + item.Power * PowerConst.V6));
            Unit unit = new Unit("temp", item, false);
            unit.Count = count;
            team.AddUnit(unit);
        }
    }

    private string RandName()
    {
        int id;
        string name;
        do
        {
            id = UnityEngine.Random.Range(1000, 10000);
            name = "AiKing_" + id;
        } while (Game.DynamicDataManager.GetGameData<King>(name) != null);
        return name;
    }

    public void AiRangeRun() //乱跑
    {
        Game.TimeManager.CreatEvetySpanMinteAction("EnemyRangeRun", 10, (action) =>
        {
            foreach (var item in Game.MapContorl.m_AllUnits)
            {
                if (item.name != "Player")
                {
                    Vector2 vector = UnityEngine.Random.insideUnitCircle * 1000;
                    item.SetTargetPostion(vector);
                }
            }
        });
    }
}