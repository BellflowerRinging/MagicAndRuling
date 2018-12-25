public class IncreaseHp : TeamEffect
{
    public IncreaseHp(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "增加队伍血量";
        Introduce = string.Format("全队的血量增加{0}%", float.Parse(value_1) * 100);
        TimeType = EffectTimeType.Time_0;
    }
    public override void DoEffect(Team team, King toking, TeamAttackingData data)
    {
        foreach (var item in team.units)
        {
            if (item == null) continue;
            item.Setting.HP = UnityEngine.Mathf.RoundToInt(Game.StaticDataManager.GetGameData<UnitSetting>(item.Setting.Name).HP * (1 + float.Parse(Values[0])));
        }
    }

    public override void Halve()
    {
        Values[0] = (float.Parse(Values[0]) * 0.5f) + "";
    }
}