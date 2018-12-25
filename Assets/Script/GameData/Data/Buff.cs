
using System;

public class Buff : GameData
{
    public string Info;
    public EffectType Type;
    public int[] EffectIDs = new int[3];
    public string[,] Values = new string[3, 3];

    public Buff(string name, bool UserSoure = true) : base(GameDataType.Other, name)
    {
        Init(UserSoure);
    }
    public Buff(GameData data, bool init = true, bool UserSoure = true) : base(data)
    {
        if (init)
            Init(UserSoure);
    }
    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];
        //if (Content.ContainsKey("Info")) Info = Content["Info"]; else SaveValue("Info", "");
        Info = InitStringValue("Info");

        if (Content.ContainsKey("Type"))
        {
            if (!string.IsNullOrEmpty(Content["Type"]))
                Type = (EffectType)int.Parse(Content["Type"]);
        }
        else SaveValue("Type", "0");


        for (int i = 0; i < EffectIDs.Length; i++)
        {
            for (int j = 0; j < Values.Length / EffectIDs.Length; j++)
            {
                if (Content.ContainsKey("Values_" + i + "_" + j))
                    Values[i, j] = Content["Values_" + i + "_" + j];
                else { SaveValue("Values_" + i + "_" + j, "0"); }
            }
        }

        for (int i = 0; i < EffectIDs.Length; i++)
        {
            // if (Content.ContainsKey("EffectIDs_" + i) && !string.IsNullOrEmpty(Content["EffectIDs_" + i]))
            //     EffectIDs[i] = int.Parse(Content["EffectIDs_" + i]);
            // else { SaveValue("EffectIDs_" + i, "-1"); }
            EffectIDs[i] = InitIntValue("EffectIDs_" + i, -1);
        }

        InitEffect();
    }

    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SaveValue("Info", Info);
        SaveValue("Type", (int)Type + "");
        for (int i = 0; i < EffectIDs.Length; i++)
        {
            SaveValue("EffectIDs_" + i, EffectIDs[i] + "");
        }
        // for (int i = 0; i < Values.Length; i++)
        // {
        // SaveValue("Values_" + i, Values[i] + "");
        // }
    }

    public virtual void InitEffect() { }
}

public class TeamBuff : Buff
{
    TeamEffect[] effects = new TeamEffect[3];
    public TeamBuff(string name, bool UserSoure = true) : base(name, UserSoure)
    {
        DataType = GameDataType.TeamBuff;
    }

    public TeamBuff(GameData data, bool init = true, bool UserSoure = true) : base(data, init, UserSoure)
    {
        DataType = GameDataType.TeamBuff;
    }

    public override void InitEffect()
    {
        for (int i = 0; i < EffectIDs.Length; i++)
        {
            if (EffectIDs[i] == -1) continue;
            string[] values = new string[5] { Values[i, 0], Values[i, 1], Values[i, 2], null, null };
            effects[i] = EffectManager.CreateEffect((TeamEffectName)EffectIDs[i], values);
        }

    }

    public void DoBuff(Team team, King king, TeamAttackingData data, EffectTimeType time)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            if (item.TimeType != time) continue;
            item.DoEffect(team, king, data);
        }
    }

    public void HalveEffect()
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            item.Halve();
        }
    }
}


public class UnitBuff : Buff
{
    UnitEffect[] effects = new UnitEffect[3];
    public UnitBuff(string name, bool UserSoure = true) : base(name, UserSoure)
    {
        DataType = GameDataType.UnitBuff;
    }

    public UnitBuff(GameData data, bool init = true, bool UserSoure = true) : base(data, init, UserSoure)
    {
        DataType = GameDataType.UnitBuff;
    }

    public override void InitEffect()
    {
        for (int i = 0; i < EffectIDs.Length; i++)
        {
            if (EffectIDs[i] == -1 || EffectIDs[i] == 0) continue;
            string[] values = new string[5] { Values[i, 0], Values[i, 1], Values[i, 2], null, null };
            effects[i] = EffectManager.CreateEffect((UnitEffectName)EffectIDs[i], values);
        }

    }

    public void DoBuff(BattleUnitData data, BattleUnitData todata, BattleData battle, EffectTimeType time)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            if (item.TimeType != time) continue;
            item.DoEffect(data, todata, battle);
        }
    }
    public void DoBuff(Unit unit, EffectTimeType time)
    {
        foreach (var item in effects)
        {
            if (item == null) continue;
            if (item.TimeType != time) continue;
            item.DoEffect(unit);
        }
    }

}
