public class OpenUnitTtan : SkillEffect
{
    public OpenUnitTtan(string value_1 = null, string value_2 = null, string value_3 = null, string value_4 = null, string value_5 = null) : base(value_1, value_2, value_3, value_4, value_5)
    {
        Name = "打开军队调整";
        Introduce = string.Format("Null Introduce");
    }

    public override void DoEffect(string skillName, King king, City city)
    {
        if (city == null) Game.UnitTranPanelContorl.Show(Game.Player);
        else Game.UnitTranPanelContorl.Show(Game.Player, city.ShopTeam);
    }
}