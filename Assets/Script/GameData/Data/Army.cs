using System.Collections;
using System.Collections.Generic;

public class Army : GameData
{
    public King king { get; private set; }
    public Team[] team = new Team[3];

    public Army(string name, bool UserSoure = true) : base(GameDataType.Army, name)
    {
        Init();
    }

    public Army(GameData data, bool init = true, bool UserSoure = false) : base(data)
    {
        if (init)
            Init(UserSoure);//
    }
    public override void Init(bool UserSoure = true)
    {
        Name = Content["0"];

        // if (Content.ContainsKey("King") && UserSoure)
        // {
        //     this.king = Game.DynamicDataManager.GetGameData<King>(Content["King"]);
        // }
        // else { SaveValue("King", ""); }
        if (UserSoure) this.king = InitParaByDynamic<King>(UserSoure, "King", "");


        // for (int i = 0; i < team.Length; i++)
        // {
        //     if (Content.ContainsKey("Team_" + i))
        //         team[i] = GameDataTool.GetOrNewGameDataByDynamic<Team>(UserSoure, Content["Team_" + i]);
        //     else { SaveValue("Team_" + i, ""); }
        // }
        team = InitParas<Team>(UserSoure, "Team", team.Length, Game.DynamicDataManager);

        for (int i = 0; i < team.Length; i++)
        {
            if (team[i] != null)
                team[i].army = this;
        }
    }

    public override void SaveToContent()
    {
        SaveValue("0", Name);
        SavePara("King", king);
        // for (int i = 0; i < team.Length; i++)
        // {
        //     if (team[i] != null) SaveValue("Team_" + i, team[i].Name);
        // }
        SaveParas("Team", team);

    }

    public override List<GameData> GetChildrens()
    {
        List<GameData> list = new List<GameData>();
        for (int i = 0; i < team.Length; i++)
        {
            list.Add(team[i]);
        }
        return list;
    }

    public void SetKing(King tking) { this.king = tking; }

    public void SetTeam(int index, Team team)
    {
        if (index < 0 || index > 2) throw new System.Exception("Error index:" + index);
        if (team != null)
        {
            team.army = this;
        }
        this.team[index] = team;
        SavePara("Teams_" + index, team);
    }

    public int GetBattleCount()
    {
        int Count = 0;
        foreach (var item in team)
        {
            if (item != null)
                Count += item.GetBattleCount();
        }
        return Count;
    }
}