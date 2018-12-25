
// public class Skill : GameData
// {
//     public string Name;
//     public int CD;
//     public Skill(string name, bool UserSoure = true) : base(GameDataType.Skill, name)
//     {
//         Init();
//     }
//     public Skill(GameData data, bool init = true, bool UserSoure = true) : base(data)
//     {
//         if (init)
//             Init();
//     }
//     public override void Init(bool UserSoure = true)
//     {
//         Name = Content["0"];
//         if (Content.ContainsKey("CD")) { CD = int.Parse(Content["CD"]); } else { SaveValue("CD", "0"); };
//     }
//     public override void SaveToContent()
//     {
//         SaveValue("0", Name);
//         SaveValue("CD", CD + "");
//     }

//     public void DoSkill()
//     {

//     }
// }
public class Skill : Buff
{
    SkillEffect[] effects = new SkillEffect[3];

    public Skill(string name, bool UserSoure = true) : base(name, UserSoure)
    {
        DataType = GameDataType.Skill;
    }

    public Skill(GameData data, bool init = true, bool UserSoure = true) : base(data, init, UserSoure)
    {
        DataType = GameDataType.Skill;
    }

    public override void InitEffect()
    {
        for (int i = 0; i < EffectIDs.Length; i++)
        {
            if (EffectIDs[i] == -1 || EffectIDs[i] == 0) continue;
            string[] values = new string[5] { Values[i, 0], Values[i, 1], Values[i, 2], null, null };
            effects[i] = EffectManager.CreateEffect((SkillEffectName)EffectIDs[i], values);
        }
    }

    public void DoSkill(King king, King toking, BattleData battle = null)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            item.DoEffect(Name, king, toking, battle);
        }
    }
    public void DoSkill(King king, King toKing)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            item.DoEffect(Name, king, toKing);
        }
    }

    public void DoSkill(King king, City toCity)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            item.DoEffect(Name, king, toCity);
        }
    }
}