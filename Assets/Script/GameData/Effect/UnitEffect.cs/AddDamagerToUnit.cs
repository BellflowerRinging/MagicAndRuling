public class AddDamagerToUnit : UnitEffect
{
    public AddDamagerToUnit(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "增加对单位的伤害";
        Introduce = string.Format("对{1}的伤害增加{0}点", value_1, value_2);
        TimeType = EffectTimeType.Time_1;
    }

    public override void DoEffect(BattleUnitData data, BattleUnitData todata, BattleData battle)
    {
        if (todata.UnitName == Values[1])
        {
            data.DoDamger = (int)(data.DoDamger + int.Parse(Values[0]));
        }
    }

    public override void DoEffect(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}