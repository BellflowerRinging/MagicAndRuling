using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleContorl
{
    public static BattlefieldUiIndex UiIndex = null;
    public King Player;
    public King S_Player;   //起保护作用的原始数据 
    private King R_Player; //原始数据 仅在战斗结束后修改

    public King Enemy;
    public King S_Enemy;
    private King R_Enemy;
    public int Round = 1;

    public BattleContorl(King PlayerKing, King EnemyKing, BattlefieldUiIndex uiIndex)
    {
        UiIndex = uiIndex;
        R_Player = PlayerKing;
        R_Enemy = EnemyKing;
        S_Player = new King(PlayerKing, true, false);
        S_Enemy = new King(EnemyKing, true, false);
        Player = new King(PlayerKing, true, false);
        Enemy = new King(EnemyKing, true, false);
        Init();
    }

    ~BattleContorl()
    {
        Debug.Log("~BattleContorl");
    }

    public void Init()
    {
        if (!UiIndex.gameObject.activeSelf) UiIndex.gameObject.SetActive(true);

        BattleTools.DoTeamBuff(Player, Enemy, null, EffectTimeType.Time_0);  //初始化BUFF Time_0

        //UiIndex = GameObject.FindObjectOfType<BattlefieldUiIndex>();
        UiIndex.m_AttackButton.onClick.RemoveAllListeners();
        UiIndex.m_EscapeButton.onClick.RemoveAllListeners();
        UiIndex.m_AttackButton_Text.text = "进攻";
        UiIndex.m_EscapeButton_Text.text = "撤退";
        UiIndex.ClearEventText();
        UiIndex.HideTableWindow();
        UiIndex.HideInfoWindow();

        UpdataBattleRanksText();
        UiIndex.ClearEventText();
        UiIndex.AddEventText("战斗开始，请下令进攻！");
        UiIndex.m_AttackButton.onClick.AddListener(Attack);
        UiIndex.m_EscapeButton.onClick.AddListener(Escape);

        UiIndex.m_Skill_1_Button.onClick.AddListener(() => { SkillButtonOnClick(0); });
        UiIndex.m_Skill_2_Button.onClick.AddListener(() => { SkillButtonOnClick(1); });
        UiIndex.m_Skill_3_Button.onClick.AddListener(() => { SkillButtonOnClick(2); });
    }

    public void UpdataBattleRanksText()
    {
        UiIndex.SetRound(Round);
        for (int i = 0; i < 3; i++)
            UiIndex.SetRankText(i, Player.army.team[i]);
        for (int i = 3; i < 6; i++)
            UiIndex.SetRankText(i, Enemy.army.team[i - 3]);
    }

    public void Escape()
    {
        //应当受到惩罚
        Defeat();
        TheEnd();
    }
    public void Attack()
    {
        TeamAttackingData[,] team_attack_datas = new TeamAttackingData[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                team_attack_datas[i, j] = CeateAttackingData(Player.army.team[i], Enemy.army.team[j], (j + i + 1), Round, this);
            }
        }

        foreach (var item in team_attack_datas)
            if (item != null) foreach (var iitem in item.UnitAttackingDatas) iitem.battleData.AllTeamAttackDatas = team_attack_datas;


        //此处根据攻击距离调整伤害
        SetDistentDamger(team_attack_datas);

        foreach (var item in team_attack_datas)
        {
            if (item != null)
                SetTeamToTeamData(item);
        }

        UiIndex.AddEventText("--{ 回合-" + Round + " }--");


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (team_attack_datas[i, j] == null) continue;
                if (team_attack_datas[i, j].DoDamger <= 0 && team_attack_datas[i, j].ByDamger <= 0) continue;

                UiIndex.AddEventText("[己方队伍" + (i + 1) + "] - [敌方队伍" + (j + 1) + "]");

                DoingUnitAttackData(team_attack_datas[i, j]);
            }
        }

        UpdataBattleRanksText();

        //BUFF 战后BUFF Time_4
        foreach (var item in team_attack_datas) BattleTools.DoTeamBuff(Player, Enemy, item, EffectTimeType.Time_4);

        DorV();

        Round++;
    }

    public void ClostBattle()
    {
        UiIndex.gameObject.SetActive(false);
        UiIndex = null;
        R_Player = null;
        R_Enemy = null;
        S_Player = null;
        S_Enemy = null;
        Player = null;
        Enemy = null;
    }

    public void SetDistentDamger(TeamAttackingData[,] team_attack_datas)
    {
        //玩家对敌人调整距离伤害
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (team_attack_datas[i, j] == null) continue;
                int locIndex = i; //当前队伍编号
                int locToIndex = j;//当前敌方队伍编号
                bool isToteam = false;
                Army army = Player.army;
                TeamAttackingData[] toTeamAttackingData = new TeamAttackingData[3];

                for (int ic = 0; ic < 3; ic++)
                {
                    toTeamAttackingData[ic] = team_attack_datas[locIndex, ic];
                }

                foreach (var item in team_attack_datas[locIndex, locToIndex].UnitAttackingDatas)
                {
                    BattleUnitData unitData = item.data;
                    BattleTools.DealWithDistent(unitData, locIndex, locToIndex, toTeamAttackingData, army, isToteam);
                }
            }
        }

        //敌人对玩家调整距离伤害
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (team_attack_datas[j, i] == null) continue;
                int locIndex = i; //当前队伍编号
                int locToIndex = j;//当前敌方队伍编号
                bool isToteam = true;
                Army army = Enemy.army;
                TeamAttackingData[] toTeamAttackingData = new TeamAttackingData[3];

                for (int ic = 0; ic < 3; ic++)
                {
                    toTeamAttackingData[ic] = team_attack_datas[ic, locIndex];
                }

                foreach (var item in team_attack_datas[locToIndex, locIndex].UnitAttackingDatas)
                {
                    BattleUnitData unitData = item.toData;
                    BattleTools.DealWithDistent(unitData, locIndex, locToIndex, toTeamAttackingData, army, isToteam);
                }
            }
        }
    }

    public void SkillButtonOnClick(int index)
    {
        if (Player.Skills[index] != null) SkillAttack(Player.Skills[index]);
    }

    public void SkillAttack(Skill skill)
    {
        if (skill == null) return;
        BattleData data = new BattleData();
        data.Round = Round;
        data.Battle = this;
        skill.DoSkill(Player, Enemy, data);
        UpdataBattleRanksText();
        DorV();
    }

    TeamAttackingData CeateAttackingData(Team team, Team toTeam, int disdent, int round, BattleContorl battle)
    {
        if (team == null || toTeam == null) return null;

        TeamAttackingData teamData = new TeamAttackingData();
        TeamSummaryData team_dic = team.GetSummary();
        teamData.Team_dic = team_dic;
        TeamSummaryData toteam_dic = toTeam.GetSummary();
        teamData.ToTeam_dic = toteam_dic;

        if (team_dic.UnitCount == 0 || toteam_dic.UnitCount == 0) return null;
        if (team_dic.Count == 0 || toteam_dic.Count == 0) return null;

        BattleData battleData = new BattleData();
        battleData.Disdent = disdent;
        battleData.Round = round;
        battleData.Battle = battle;

        teamData.Team = team;
        teamData.ToTeam = toTeam;
        teamData.battleData = battleData;
        List<UnitAttackingData> unitDatas = CreateUnitToUnitDatas(team, toTeam, team_dic, toteam_dic, battleData);

        teamData.UnitAttackingDatas = unitDatas;
        foreach (var item in unitDatas) item.teamData = teamData;

        //SetTeamToTeamData(teamData);

        return teamData;
    }

    private void SetTeamToTeamData(TeamAttackingData teamData)
    {

        TeamSummaryData team_dic = teamData.Team_dic;
        TeamSummaryData toteam_dic = teamData.ToTeam_dic;
        List<UnitAttackingData> unitDatas = teamData.UnitAttackingDatas;

        //buff1 例如调整特定单位对特定单位的伤害 调整伤害的BUFF                        Time_1        
        foreach (var item in unitDatas) BattleTools.DoUnitBuff(item, EffectTimeType.Time_1);

        int all_damger = BattleTools.GetDameger(unitDatas);         //造成总伤害
        int all_t_damger = BattleTools.GetByDameger(unitDatas);     //对方的造成总伤害

        // 此处调整受伤率

        // 根据各种因素调整伤害
        SetDamager(all_damger, all_t_damger, team_dic, toteam_dic, unitDatas);

        //buff3 例如调整特定单位对特定单位的保护效果 真实伤害 调整受到伤害的BUFF     Time_2
        foreach (var item in unitDatas) BattleTools.DoUnitBuff(item, EffectTimeType.Time_2);

        teamData.DoDamger = BattleTools.GetDameger(unitDatas);   //总实际伤害
        teamData.ByDamger = BattleTools.GetByDameger(unitDatas);
    }

    public static void SetDamager(int all_damger, int all_t_damger, TeamSummaryData team_dic, TeamSummaryData toteam_dic, List<UnitAttackingData> teamDatas)
    {
        float injpr = BattleTools.GetInjurePr(all_t_damger, team_dic, toteam_dic);
        float t_injpr = BattleTools.GetInjurePr(all_damger, toteam_dic, team_dic);

        int all_Ed_damger = 0, avg_Ed_damger = 0, all_t_Ed_damger = 0, avg_t_Ed_damger = 0;

        if (team_dic != null)
        {
            all_Ed_damger = BattleTools.GetEdDamager(all_t_damger, team_dic);    //减少的总伤害
            avg_Ed_damger = (int)((float)all_Ed_damger / (float)team_dic.UnitCount);   //平均减少的伤害  所有单位共享防御
        }

        if (toteam_dic != null)
        {
            all_t_Ed_damger = BattleTools.GetEdDamager(all_damger, toteam_dic);
            avg_t_Ed_damger = (int)((float)all_t_Ed_damger / (float)toteam_dic.UnitCount);
        }

        foreach (var item in teamDatas) //更新伤害
        {
            item.data.DoDamger = Mathf.Clamp(item.data.DoDamger - avg_t_Ed_damger, 0, int.MaxValue);
            item.toData.DoDamger = Mathf.Clamp(item.toData.DoDamger - avg_Ed_damger, 0, int.MaxValue);
            item.data.InjuryPr = injpr;
            item.toData.InjuryPr = t_injpr;
        }
    }

    private List<UnitAttackingData> CreateUnitToUnitDatas(Team team, Team toTeam, TeamSummaryData teamsum, TeamSummaryData toTeamsum, BattleData battleData)
    {
        List<UnitAttackingData> teamDatas = new List<UnitAttackingData>();
        foreach (var unit in team.units)
        {
            foreach (var tounit in toTeam.units)
            {
                if (unit == null || tounit == null) continue;
                UnitAttackingData data = new UnitAttackingData();
                data.data.UnitName = unit.Setting.Name;
                data.toData.UnitName = tounit.Setting.Name;

                UnitSummaryData untisum = unit.GetSummary();
                UnitSummaryData tountisum = tounit.GetSummary();

                data.data.DoDamger = (int)(untisum.Ad * ((float)tountisum.Count / (float)toTeamsum.Count));
                data.toData.DoDamger = (int)(tountisum.Ad * ((float)untisum.Count / (float)teamsum.Count));

                data.data.DoDamger = Mathf.Clamp(data.data.DoDamger, 0, int.MaxValue);
                data.toData.DoDamger = Mathf.Clamp(data.toData.DoDamger, 0, int.MaxValue);

                data.data.Unit = unit;
                data.toData.Unit = tounit;

                teamDatas.Add(data);
            }
        }
        return teamDatas;
    }

    public static List<UnitAttackingData> CreateSkillToUnitData(string name, int damager, Team toTeam, TeamSummaryData toTeamsum, BattleData battledata)
    {
        List<UnitAttackingData> teamDatas = new List<UnitAttackingData>();

        foreach (var tounit in toTeam.units)
        {
            if (tounit == null) continue;
            UnitAttackingData data = new UnitAttackingData();
            data.toData.UnitName = tounit.Setting.Name;

            UnitSummaryData tountisum = tounit.GetSummary();

            data.data.UnitName = name;
            data.data.DoDamger = (int)(damager * ((float)tountisum.Count / (float)toTeamsum.Count));
            data.data.DoDamger = Mathf.Clamp(data.data.DoDamger, 0, int.MaxValue);

            data.toData.Unit = tounit;
            data.battleData = battledata;

            teamDatas.Add(data);
        }

        return teamDatas;
    }

    public static void DoingUnitAttackData(TeamAttackingData data)
    {

        int c_inj = 0, c_death = 0;
        int t_inj = 0, t_death = 0;

        foreach (var item in data.UnitAttackingDatas)
        {
            if (item == null) continue;
            BattleTools.DoDameger(item);

            //BUFF Time_3
            BattleTools.DoUnitBuff(item, EffectTimeType.Time_3);

            c_inj += item.data.Injury;
            c_death += item.data.Death;
            t_inj += item.toData.Injury;
            t_death += item.toData.Death;
        }

        UiIndex.AddEventText(string.Format("对敌人造成{0}点伤害，击伤{1}人，击杀{2}人",
             data.DoDamger, t_inj, t_death));

        UiIndex.AddEventText(string.Format("己方受到{0}点伤害，受伤{1}人，死亡{2}人\n",
             data.ByDamger, c_inj, c_death));
    }

    private void DorV()
    {
        if (Player.army.GetBattleCount() == 0)
        {
            Defeat();
            TheEnd();
        }
        else if (Enemy.army.GetBattleCount() == 0)
        {
            victory();
            TheEnd();
        }
    }

    private void TheEnd()
    {
        UiIndex.m_AttackButton.onClick.AddListener(ClostBattle);
        UiIndex.m_EscapeButton.onClick.AddListener(ClostBattle);
        UiIndex.m_AttackButton_Text.text = "结束";
        UiIndex.m_EscapeButton_Text.text = "结束";

        /* 暂时不保存
        SaveUnit(R_Player, Player);
        SaveUnit(R_Enemy, Enemy);
        Game.DynamicDataManager.Save();
        */
    }

    private void SaveUnit(King R_king, King king)
    {
        for (int i = 0; i < R_king.army.team.Length; i++)
        {
            Team r_team = R_king.army.team[i];
            Team team = king.army.team[i];

            if (r_team == null) continue;
            r_team.RemoveAllUnit(true);

            foreach (var item in team.units)
            {
                r_team.AddUnit(item, true);
            }
        }
    }

    private void Defeat()
    {
        UiIndex.ShowInfoWindow("战败！");
    }

    private void victory()
    {
        UiIndex.ShowInfoWindow("胜利！");
    }







}



