using System.Collections.Generic;

public class UnitSetting : GameData
{
    public int HP { get; set; }
    public int AD { get; set; }
    public int DE { get; set; }
    public int AttackRange { get; set; }
    public string Introduce { get; set; }
    public string BuffIntroduce { get; set; }
    public UnitBuff[] buff = new UnitBuff[5];
    public int Price { get; set; }
    public UnitSetting(string name, bool UserSoure = true) : base(GameDataType.UnitSetting, name)
    {
        Init(UserSoure);
    }
    public UnitSetting(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure)
    {
        Name = Content["0"];
        // if (Content.ContainsKey("HP")) { HP = int.Parse(Content["HP"]); } else { SaveValue("HP", "0"); };
        // if (Content.ContainsKey("AD")) { AD = int.Parse(Content["AD"]); } else { SaveValue("AD", "0"); };
        // if (Content.ContainsKey("DE")) { DE = int.Parse(Content["DE"]); } else { SaveValue("DE", "0"); };
        // if (Content.ContainsKey("AttackRange")) { AttackRange = int.Parse(Content["AttackRange"]); } else { SaveValue("AttackRange", "0"); };
        // if (Content.ContainsKey("Introduce")) Introduce = Content["Introduce"]; else SaveValue("Introduce", "");
        HP = InitIntValue("HP");
        AD = InitIntValue("AD");
        DE = InitIntValue("DE");
        AttackRange = InitIntValue("AttackRange");
        Introduce = InitStringValue("Introduce");

        Price = InitIntValue("Price");

        for (int i = 0; i < buff.Length; i++)
        {
            // if (Content.ContainsKey("Buff_" + i)) buff[i] = Game.DynamicDataManager.GetGameData<UnitBuff>(Content["Buff_" + i]);
            // else SaveValue("Buff_" + i, "");
            buff[i] = InitParaByDynamic<UnitBuff>(UserSoure, "Buffs_" + i);
            if (buff[i] != null)
            {
                BuffIntroduce += buff[i].Info + "\n";
            }
        }
    }
    public override void SaveToContent() { }

    public int Power
    {
        get
        {
            int s = (AD * PowerConst.V1 + DE * PowerConst.V2 + HP * PowerConst.V3);
            // Game.EventTextContorl.AddEventTextLn(string.Format(
            //     "AD:{0} DE:{1} HP:{2} ",
            //      AD * PowerConst.V1, DE * PowerConst.V2, HP * PowerConst.V3, s));
            return s;
        }
    }
}