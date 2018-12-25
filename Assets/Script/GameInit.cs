using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameInit : MonoBehaviour
{

    public GameEventTextContorl m_GameEventTextContorl;
    public CommunicationContorl m_MustCommunicationContorl;
    public CityCommunicationContorl m_CityCommunicationContorl;
    public BattlefieldUiIndex m_BattleUiindex;
    public UnitTablePage m_page;
    public UnitTranPanelContorl m_UnitTranPanelContorl;
    public MapContorl m_MapCotorl;
    public GameTimeContorl m_TimeContorl;
    // public DynamicGameSetting m_DynamicGameSetting;
    // public StaticGameSetting m_StaticGameSetting;
    // Use this for initialization
    void Start()
    {
        Game.SetGameEventTextContorl(m_GameEventTextContorl);
        Game.SetMustCommunicationContorl(m_MustCommunicationContorl);
        Game.SetCityCommunicationContorl(m_CityCommunicationContorl);
        Game.SetUnitTranPanelContorl(m_UnitTranPanelContorl);
        Game.SetBattleUiIndex(m_BattleUiindex);
        Game.SetMapContorl(m_MapCotorl);
        Game.SetTimeManager(m_TimeContorl);
        // Game.SetDynamicSetting(m_DynamicGameSetting);
        // Game.SetStaticSetting(m_StaticGameSetting);
        Game.Init();
        // Game.MustCommunicationContorl.ShowCommunication(null, Game.StaticDataManager.GetGameData<CommunicationData>("Comm_1"));
        //Game.CityCommunicationContorl.ShowCommunication(null, Game.StaticDataManager.GetGameData<CommunicationData>("Comm_1"));

        King king = Game.DynamicDataManager.GetGameData<King>("Player");
        King toking = Game.DynamicDataManager.GetGameData<King>("Enemy");
        Game.SetPlayer(king);
        //m_page.ShowUnitList(new List<Unit>(king.army.team[0].units));
        //Game.CreateBattle(king, toking);
        //m_UnitTranPanelContorl.Show(king);
        Game.MapContorl.PlayerUnit.SetKing(Game.Player);

        //Game.TimeManager.CreatEvetySpanMinteAction("Test", 3, (name) => { Debug.Log(Game.TimeManager.time.Minute); });
        //Game.TimeManager.CreatTimerActionWaitForMinute("Timer", 5, (th) => { Debug.Log("Hello world"); });

        //Game.AiManager.AiRangeRun();

        //Debug.Log(Game.Player.Power);
        //Debug.Log(Game.DynamicDataManager.GetGameData<King>("Enemy").Power);

         Game.AiManager.CreateAiKingRangeMapPostion();
         Game.AiManager.CreateAiKingRangeMapPostion();
         Game.AiManager.CreateAiKingRangeMapPostion();
         Game.AiManager.CreateAiKingRangeMapPostion();
         Game.AiManager.CreateAiKingRangeMapPostion();
         Game.AiManager.CreateAiKingRangeMapPostion();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
