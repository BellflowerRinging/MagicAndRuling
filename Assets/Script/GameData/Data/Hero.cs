using System.Collections;
using System.Collections.Generic;

public class Hero : GameData
{
    public string ShowName;
    public King king;
    public Team team { get; private set; }
    //public List<string> Buff;
    public string Introduce = "";
    public TeamBuff[] buff = new TeamBuff[5];

    public Hero(string name, bool UserSoure = true) : base(GameDataType.Hero, name)
    {
        Init(UserSoure);
    }
    public Hero(GameData data, bool init = true, bool UserSoure = true) : base(data)
    {
        if (init)
            Init(UserSoure);//
    }
    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];

        //if (Content.ContainsKey("King") && UserSoure) { this.king = Game.DynamicDataManager.GetGameData<King>(Content["King"]); } else { SaveValue("King", ""); }
        if (UserSoure) king = InitParaByDynamic<King>(UserSoure, "King");
        //if (Content.ContainsKey("Team") && UserSoure) { this.team = Game.DynamicDataManager.GetGameData<Team>(Content["Team"]); } else { SaveValue("Team", ""); }
        if (UserSoure) team = InitParaByDynamic<Team>(UserSoure, "Team");

        // if (Content.ContainsKey("Introduce")) Introduce = Content["Introduce"]; else SaveValue("Introduce", "");
        // if (Content.ContainsKey("ShowName")) ShowName = Content["ShowName"]; else SaveValue("ShowName", "");
        Introduce = InitStringValue("Introduce");
        ShowName = InitStringValue("ShowName");

        if (king != null && team != null)
            if (team.army != null)
                if (team.army.king != king) throw new System.Exception("team.army.king != king");

        if (this.king == null && this.team != null)
            this.king = this.team.army.king;

        // if (Content.ContainsKey("ShowName")) { ShowName = Content["ShowName"]; } else { SaveValue("ShowName", "英雄"); };

        // for (int i = 0; i < buff.Length; i++)
        // {
        //     if (Content.ContainsKey("Buff_" + i))
        //         buff[i] = GameDataTool.GetOrNewGameDataByDynamic<TeamBuff>(UserSoure, Content["Buff_" + i]);
        //     else { SaveValue("Buff_" + i, ""); }
        // }
        buff = InitParas<TeamBuff>(UserSoure, "Buff", buff.Length, Game.DynamicDataManager);

    }
    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SaveValue("ShowName", ShowName);
        SaveValue("Introduce", Introduce);
        SavePara("King", king);
        SavePara("Team", team);
        SaveParas("Buff", buff);
        // for (int i = 0; i < buff.Length; i++)
        // {
        //     if (buff[i] != null) SaveValue("Buff_" + i, buff[i].Name);
        // }
    }

    public override List<GameData> GetChildrens()
    {
        List<GameData> list = new List<GameData>();
        for (int i = 0; i < buff.Length; i++)
        {
            list.Add(buff[i]);
        }
        return list;
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        if (team != null && team.army != null)
            king = team.army.king;
    }

    public void DoBuff(King toKing, TeamAttackingData data, EffectTimeType time)
    {
        foreach (var item in buff)
        {
            if (item == null) continue;
            item.DoBuff(team, toKing, data, time);
        }
    }
}