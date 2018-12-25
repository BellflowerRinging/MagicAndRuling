
public enum EffectType
{
    Unit = 1,
    Team = 2,
    Skill = 3
}
public enum UnitEffectName
{
    AddDamagerToUnit = 1
}

//还需更新 CreateEffect

public enum TeamEffectName
{
    IncreaseHp = 1,
}

public enum SkillEffectName
{
    DamegerToKing = 1,
    OpenUnitTtan = 2,
    AttackEffect = 3,
}


public enum EffectTimeType
{
    Time_0 = 0,
    Time_1 = 1,
    Time_2 = 2,
    Time_3 = 3,
    Time_4 = 4
}

public class EffectManager
{
    public static TeamEffect CreateEffect(TeamEffectName teamEffectName, string[] values)
    {
        switch (teamEffectName)
        {
            case TeamEffectName.IncreaseHp: return new IncreaseHp(values[0], values[1], values[2], values[3], values[4]);
            default: return null;
        }
    }

    public static UnitEffect CreateEffect(UnitEffectName UnitEffectName, string[] values)
    {
        switch (UnitEffectName)
        {
            case UnitEffectName.AddDamagerToUnit: return new AddDamagerToUnit(values[0], values[1], values[2], values[3], values[4]);
            default: return null;
        }
    }

    public static SkillEffect CreateEffect(SkillEffectName SkillEffectName, string[] values)
    {
        switch (SkillEffectName)
        {
            case SkillEffectName.DamegerToKing: return new DamegerToKing(values[0], values[1], values[2], values[3], values[4]);
            case SkillEffectName.OpenUnitTtan: return new OpenUnitTtan(values[0], values[1], values[2], values[3], values[4]);
            case SkillEffectName.AttackEffect: return new AttackEffect(values[0], values[1], values[2], values[3], values[4]);
            default: return null;
        }
    }
}

public abstract class Effect
{
    public EffectType Type;
    public EffectTimeType TimeType;
    public string Name
    { get; protected set; }
    public string Introduce { get; protected set; }
    public string[] Values { get; protected set; }
    protected Effect(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null)
    {
        Name = "未知效果";
        Introduce = "尚未了解清楚该效果会发生什么";
        Values = new string[] { value_1, value_2, value_3, value_4, value_5 };
    }
}

public abstract class TeamEffect : Effect
{
    public TeamEffect(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "未知队伍效果";
        Introduce = "尚未了解清楚该队伍效果会发生什么";
        Type = EffectType.Team;
    }
    public abstract void DoEffect(Team team, King toking, TeamAttackingData data);
    abstract public void Halve();
}

public class SkillEffect : Effect
{
    public SkillEffect(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "未知技能效果";
        Introduce = "尚未了解清楚该技能效果会发生什么";
        Type = EffectType.Skill;
    }
    public virtual void DoEffect(string skillName, King king, King toking, BattleData data)
    {
        throw new System.MissingMethodException();
    }
    public virtual void DoEffect(string skillName, King king, King toking)
    {
        throw new System.MissingMethodException();
    }
    public virtual void DoEffect(string skillName, King king, City city)
    {
        throw new System.MissingMethodException();
    }

}

public abstract class UnitEffect : Effect
{
    public UnitEffect(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "未知单位效果";
        Introduce = "尚未了解清楚该单位效果会发生什么";
        Type = EffectType.Unit;
    }
    public abstract void DoEffect(BattleUnitData data, BattleUnitData todata, BattleData battle);

    public abstract void DoEffect(Unit unit);
}