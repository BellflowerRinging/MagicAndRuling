public class AttackEffect : SkillEffect
{
    public AttackEffect(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "战斗";
        Introduce = string.Format("Null Introduce");
    }

    public override void DoEffect(string skillName, King king, King toKing)
    {
        if (king == null) throw new System.Exception("king == null");
        if (toKing == null) throw new System.Exception("toKing == null");
        Game.CreateBattle(Game.Player, toKing);
    }
}