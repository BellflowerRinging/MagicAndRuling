
using System.Collections.Generic;

public class Game
{
    static public King Player { get; private set; }
    static public DataManager DynamicDataManager { get; private set; }
    static public DataManager StaticDataManager { get; private set; }
    static public GameEventTextContorl EventTextContorl { get; private set; }
    static public CommunicationContorl MustCommunicationContorl { get; private set; }
    static public CityCommunicationContorl CityCommunicationContorl { get; private set; }
    static public UnitTranPanelContorl UnitTranPanelContorl { get; private set; }
    static public BattlefieldUiIndex BattleUiIndex { get; private set; }
    static public MapContorl MapContorl { get; private set; }
    static public GameSetting Setting { get; private set; }
    static public GameTimeManager TimeManager { get; private set; }
    static public AiManager AiManager { get; private set; }

    public struct GameSetting
    {
        public DynamicGameSetting Dynamic { get; set; }
        public StaticGameSetting Static { get; set; }
        public GameSetting(DynamicGameSetting Dsetting, StaticGameSetting Ssetting)
        {
            Dynamic = Dsetting;
            Static = Ssetting;
        }
    }

    public static void Init()
    {
        InitDataManager();
        InitGameSetting(DynamicDataManager.GetGameData<DynamicGameSetting>("GameSetting"), StaticDataManager.GetGameData<StaticGameSetting>("GameSetting"));
        MapContorl.Init();
        TimeManager.SetDynGameSetting(Setting.Dynamic);
        AiManager = new AiManager();
        // BattleContorl contorl =
        //     new BattleContorl(Game.DynamicDataManager.GetGameData<King>("Player"),
        //                          Game.DynamicDataManager.GetGameData<King>("Enemy"));
    }

    private static void InitGameSetting(DynamicGameSetting dynamicGameSetting, StaticGameSetting staticGameSetting)
    {
        Setting = new GameSetting(dynamicGameSetting, staticGameSetting);
    }

    public static void SetGameEventTextContorl(GameEventTextContorl contorl)
    {
        if (EventTextContorl == null)
            EventTextContorl = contorl;
    }

    public static void SetMustCommunicationContorl(CommunicationContorl contorl)
    {
        if (MustCommunicationContorl == null)
            MustCommunicationContorl = contorl;
    }

    public static void SetCityCommunicationContorl(CityCommunicationContorl contorl)
    {
        if (CityCommunicationContorl == null)
            CityCommunicationContorl = contorl;
    }

    public static void SetUnitTranPanelContorl(UnitTranPanelContorl contorl)
    {
        if (UnitTranPanelContorl == null)
            UnitTranPanelContorl = contorl;
    }

    public static void SetMapContorl(MapContorl contorl)
    {
        if (MapContorl == null)
            MapContorl = contorl;
    }

    public static void SetPlayer(King king)
    {
        if (Player == null)
            Player = king;
    }

    public static void SetBattleUiIndex(BattlefieldUiIndex index)
    {
        BattleUiIndex = index;
    }

    public static void SetTimeManager(GameTimeContorl contorl)
    {
        if (TimeManager == null)
        {
            TimeManager = new GameTimeManager();
            TimeManager.SetTimeContorl(contorl);
        }
    }



    public static void CreateBattle(King king, King to_King)
    {
        new BattleContorl(king, to_King, BattleUiIndex);
    }

    public static void InitDataManager()
    {
        List<GameDataType> Filter = new List<GameDataType>();
        Filter.Add(GameDataType.King);
        Filter.Add(GameDataType.Army);
        Filter.Add(GameDataType.Team);
        Filter.Add(GameDataType.Hero);
        Filter.Add(GameDataType.Unit);
        Filter.Add(GameDataType.Skill);
        Filter.Add(GameDataType.TeamBuff);
        Filter.Add(GameDataType.UnitBuff);
        Filter.Add(GameDataType.City);
        Filter.Add(GameDataType.DynamicGameSetting);
        Filter.Add(GameDataType.Other);
        DynamicDataManager = new DataManager("DynamicData", false, Filter);
        Filter = new List<GameDataType>();
        Filter.Add(GameDataType.UnitSetting);
        Filter.Add(GameDataType.UnitEffect);
        Filter.Add(GameDataType.TeamEffect);
        Filter.Add(GameDataType.SkillEffect);
        Filter.Add(GameDataType.ConversationData);
        Filter.Add(GameDataType.StaticGameSetting);
        Filter.Add(GameDataType.Other);
        StaticDataManager = new DataManager("StaticGameData", true, Filter);

        StaticDataManager.Load();
        DynamicDataManager.Load();
        StaticDataManager.InitGameData();
        DynamicDataManager.InitGameData();
    }
}