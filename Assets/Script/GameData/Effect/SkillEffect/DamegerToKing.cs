using System.Collections.Generic;

public class DamegerToKing : SkillEffect
{
    public DamegerToKing(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "对敌方造成伤害";
        Introduce = string.Format("对敌方造成伤害{0}%点伤害", value_1);
    }

    public override void DoEffect(string skillName, King king, King toking, BattleData battleData)
    {
        int damager = int.Parse(Values[0]);
        List<TeamAttackingData> team_attack_datas = new List<TeamAttackingData>();


        foreach (var team in toking.army.team)
        {
            if (team == null) continue;
            if (team.GetBattleCount() == 0) continue;
            TeamAttackingData teamData = new TeamAttackingData();
            List<UnitAttackingData> list = new List<UnitAttackingData>();

            TeamSummaryData data = team.GetSummary();
            list = BattleContorl.CreateSkillToUnitData(skillName, damager, team, data, battleData);

            //调整伤亡率
            BattleContorl.SetDamager(damager, 0, null, data, list);

            //buff3 例如调整特定单位对特定单位的保护效果 真实伤害 调整受到伤害的BUFF     Time_2
            foreach (var unitdata in list) BattleTools.DoUnitBuff(unitdata, EffectTimeType.Time_2);

            teamData.DoDamger = BattleTools.GetDameger(list);
            team_attack_datas.Add(teamData);
            teamData.UnitAttackingDatas = list;

        }

        foreach (var item in team_attack_datas)
        {
            BattleContorl.DoingUnitAttackData(item);
        }

        //BUFF 战后BUFF Time_4
        foreach (var item in team_attack_datas) BattleTools.DoTeamBuff(king, toking, item, EffectTimeType.Time_4);

    }
}