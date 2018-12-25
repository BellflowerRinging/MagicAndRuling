using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTools
{
    public static void DealWithDistent(BattleUnitData unitData, int locIndex, int locToIndex, TeamAttackingData[] toTeamAttackingData, Army army, bool isToteam)
    {
        int around = unitData.Unit.Setting.AttackRange - locIndex;

        for (int c = 0; c < around && c < 3; c++) //大于四队时可能会发生错误
        {
            if (toTeamAttackingData[c] == null)  //跳过敌方空队伍
                around++;
        }

        for (int c = 0; c < locIndex; c++) //跳过己方空队伍
        {
            if (army.team[c] == null || army.team[c].GetSummary().UnitCount == 0 || army.team[c].GetSummary().Count == 0)
                around++;
        }

        if (locToIndex + 1 > around) //距离不够置空伤害
        {
            unitData.DoDamger = 0;
            return;
        }
        if (around <= 0)
        {
            unitData.DoDamger = 0;
            return;
        }
        else if (around == 1) return;

        around = Mathf.Clamp(around, 0, 2);
        //float damger = unitData.DoDamger;F
        List<TeamAttackingData> list = new List<TeamAttackingData>();

        for (int c = 0; c < around; c++)  //获取攻击范围内队伍
            if (toTeamAttackingData[c] != null) list.Add(toTeamAttackingData[c]);

        if (list.Count <= 1) return;

        unitData.DoDamger = (int)(unitData.DoDamger / list.Count); //应当向上或下取整  伤害由范围内的队伍平均分担

        // int unitCount = 0;   //伤害根据敌方人数平均分担
        // foreach (var team in list)
        //     if (!isToteam)
        //         unitCount += team.ToTeam_dic.Count;
        //     else
        //         unitCount += team.Team_dic.Count;
        //unitData.DoDamger = (int)(unitData.DoDamger * ((float)team_attack_datas[i, j].ToTeam_dic.Count / (float)unitCount)); //应当向上或下取整
    }

    public static void DoDameger(UnitAttackingData item)
    {
        DoingDameger(item.toData.DoDamger, item.data.Unit, item.data.InjuryPr, ref item.data.Injury, ref item.data.Death);

        DoingDameger(item.data.DoDamger, item.toData.Unit, item.toData.InjuryPr, ref item.toData.Injury, ref item.toData.Death);
    }

    public static void DoingDameger(int ByDamger, Unit Unit, float injpr, ref int inj, ref int death)
    {
        if (Unit == null) return;
        int a_count = Mathf.CeilToInt((float)ByDamger / (float)Unit.Setting.HP); //预计死亡人数 
        a_count = Mathf.Clamp(a_count, 0, Unit.Count - Unit.Injury);
        inj = Mathf.CeilToInt((a_count * injpr));// *0.5 受伤率 
        death = (a_count - inj); //实际死亡人数
        Unit.Injury += inj;
        Unit.Count -= death;
    }

    public static void DoTeamBuff(King Player, King Enemy, TeamAttackingData data, EffectTimeType time)
    {
        //throw new NotImplementedException();
        //((TeamBuff)(Player.army.team[0].MainHero.buff[0])).DoBuff(Player.army.team[0], Enemy);
        foreach (var item in Player.army.team)
        {
            if (item == null) continue;
            item.DoMainHeroBuff(Enemy, data, time);
            item.DoMinorHeroBuff(Enemy, data, time);
            foreach (var unit in item.units)
            {
                if (unit == null) continue;
                unit.DoBuff(time);
            }
        }

        foreach (var item in Enemy.army.team)
        {
            if (item == null) continue;
            item.DoMainHeroBuff(Player, data, time);
            item.DoMinorHeroBuff(Player, data, time);
            foreach (var unit in item.units)
            {
                if (unit == null) continue;
                unit.DoBuff(time);
            }
        }
    }
   
    public static void DoUnitBuff(UnitAttackingData item, EffectTimeType time)
    {
        if (item.data.Unit != null)
            item.data.Unit.DoBuff(item.data, item.toData, item.battleData, time);
        if (item.toData.Unit != null)
            item.toData.Unit.DoBuff(item.toData, item.data, item.battleData, time);
    }

    public static int GetEdDamager(int Dameger, TeamSummaryData data)
    {
        return data.Ed;
    }

    public static float GetInjurePr(int Dameger, TeamSummaryData data, TeamSummaryData todata)
    {
        // data==null skill 
        return 0.5f;
    }

    static public int GetDameger(List<UnitAttackingData> who)
    {
        int damger = 0;
        foreach (var item in who)
        {
            damger += item.data.DoDamger;
        }
        return damger;
    }

    static public int GetByDameger(List<UnitAttackingData> who)
    {
        int damger = 0;
        foreach (var item in who)
        {
            damger += item.toData.DoDamger;
        }
        return damger;
    }

    static public void SetEdByDameger(List<UnitAttackingData> who, int avg_damger)
    {
        foreach (var item in who)
        {
            item.toData.DoDamger -= avg_damger;
        }
    }

    public static void SetRankText(Rank rank, Team team, Action<Rank, Team> RegistTeamButton)
    {
        rank.Hero_1_Text.text = "";
        rank.Hero_2_Text.text = "";
        rank.Army_Text_0.text = "";
        rank.Army_Text_1.text = "";

        if (team != null)
        {
            if (team.MainHero != null) rank.Hero_1_Text.text = team.MainHero.ShowName;
            if (team.MinorHero != null) rank.Hero_2_Text.text = team.MinorHero.ShowName;
            TeamSummaryData dic = team.GetSummary();
            if (dic.Maxhp == 0) return;
            string str;
            str = string.Format("Hp:{0}/{1}\n攻击:{2}\n人数:{3}", dic.Hp, dic.Maxhp, dic.Ad, dic.Count);
            rank.Army_Text_0.text = str;
            str = string.Format("士气:{0}\n防御:{1}\n受伤:{2}", dic.Morale, dic.Ed, dic.Injury);
            rank.Army_Text_1.text = str;
            RegistTeamButton(rank, team);
        }

    }



}

[System.Serializable]
public class Rank
{
    public Button Hero_1_Button;
    public Text Hero_1_Text;
    public Button Hero_2_Button;
    public Text Hero_2_Text;
    public Button Army_Button;
    public Text Army_Text_0;
    public Text Army_Text_1;
}

public class TeamAttackingData
{
    public string TeamName;
    public int DoDamger;
    public int ByDamger;
    //public BattleUnitData UnitData;
    public Team Team;
    public TeamSummaryData Team_dic;
    public Team ToTeam;
    public TeamSummaryData ToTeam_dic;
    public List<UnitAttackingData> UnitAttackingDatas;
    public BattleData battleData = new BattleData();

}


public class UnitAttackingData
{
    public BattleUnitData data = new BattleUnitData();
    public BattleUnitData toData = new BattleUnitData();
    public BattleData battleData = new BattleData();

    public TeamAttackingData teamData;

}

public class BattleData
{
    public int Disdent;
    public int Round;
    internal BattleContorl Battle;
    public TeamAttackingData[,] AllTeamAttackDatas;
}

public class BattleUnitData
{
    public string UnitName;
    public int DoDamger;
    public float InjuryPr;
    public int Injury;
    public int Death;
    public Unit Unit;
}

// TeamAttackingData teamAttackingData = team_attack_datas[i, j];
// if (teamAttackingData == null) continue;
// int nullCount = 0;


// foreach (var item in teamAttackingData.UnitAttackingDatas)
// {
//     int around = item.data.Unit.Setting.AttackRange - i;
//     for (int c = 0; c < 2; c++)
//     {
//         if (team_attack_datas[i, c] == null) around++;
//     }
//     for (int t = 0; t < i; i++)
//     {

//     }
//     if (around >= 4) continue;

//     if (around <= 0)
//     {
//         item.data.DoDamger = 0;
//     }
//     else if (around == 1)
//     {
//         if (j == 1 && team_attack_datas[i, 0] != null) item.data.DoDamger = 0;
//         if (j == 2 && team_attack_datas[i, 0] != null && team_attack_datas[i, 1] != null) item.data.DoDamger = 0;
//     }
//     else if (around == 2)
//     {
//         if (j == 2 && team_attack_datas[i, 0] != null && team_attack_datas[i, 1] != null) item.data.DoDamger = 0;
//         else
//         {
//             int locCount = teamAttackingData.ToTeam_dic.UnitCount;
//             int aroundCount = 0;
//             if (team_attack_datas[i, 0] == null)
//             {
//                 aroundCount = team_attack_datas[i, 1].ToTeam_dic.UnitCount + team_attack_datas[i, 2].ToTeam_dic.UnitCount;
//             }
//             else if (team_attack_datas[i, 1] == null)
//             {
//                 aroundCount = team_attack_datas[i, 0].ToTeam_dic.UnitCount + team_attack_datas[i, 2].ToTeam_dic.UnitCount;
//             }
//             float pr = locCount / aroundCount;
//             item.data.DoDamger = (int)(item.data.DoDamger * pr);
//         }
//     }
// }