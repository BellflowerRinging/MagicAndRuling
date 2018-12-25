using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTranPanelContorl : MonoBehaviour
{

    public GameObject m_InfoWindow;
    public Text m_InfoWindow_Text;
    public Button m_InfosCloseButton;
    public Button m_EnterButton;
    public Button m_ResetButton;
    public Button m_CanelButton;
    public UnitTablePage m_TablePage_0;
    public UnitTablePage m_TablePage_1;
    public Text m_Money;

    public Button m_ToggleButton_0;
    public Button m_ToggleButton_1;
    // Use this for initialization
    public Rank[] Ranks;

    public King R_Player;
    public Team R_ShopTeam;

    public King Player;
    public Team ShopTeam;
    void Start()
    {
        m_InfosCloseButton.onClick.AddListener(() =>
        {
            CloseInfoWindow();
        });

        m_EnterButton.onClick.AddListener(() =>
        {
            EnterTrans();
        });

        m_ResetButton.onClick.AddListener(() =>
        {
            RetSetTrans();
        });

        m_CanelButton.onClick.AddListener(() =>
        {
            CancelTrans();
        });

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(King king, Team shopTeam = null)
    {
        R_Player = king;
        R_ShopTeam = shopTeam;
        Player = new King(R_Player, true, false);
        if (R_ShopTeam != null) ShopTeam = new Team(R_ShopTeam, true, false);

        m_ToggleButton_0.onClick.RemoveAllListeners();
        m_ToggleButton_1.onClick.RemoveAllListeners();
        if (ShopTeam == null) InitPlayerUnitAdjust();
        else InitShopUnitAdjust();

        Flush();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void EnterTrans()
    {
        for (int i = 0; i < 3; i++)
        {
            Team r_team = R_Player.army.team[i];
            Team s_team = Player.army.team[i];

            if (r_team != null)
                foreach (var item in r_team.units)
                    if (item != null)
                        Game.DynamicDataManager.DelGameData(item);
            Game.DynamicDataManager.DelGameData(r_team);

            R_Player.army.team[i] = s_team;
            Game.DynamicDataManager.AddGameData(s_team);
        }

        //
        if (ShopTeam != null && R_ShopTeam != null)
        {
            if (R_ShopTeam != null)
                foreach (var item in R_ShopTeam.units)
                    if (item != null)
                        Game.DynamicDataManager.DelGameData(item);
            Game.DynamicDataManager.DelGameData(R_ShopTeam);

            Game.DynamicDataManager.AddGameData(ShopTeam);
        }

        R_Player.Money = Player.Money;

        Game.DynamicDataManager.Save();
    }

    public void RetSetTrans()
    {
        Player = new King(R_Player, true, false);
        if (R_ShopTeam != null) ShopTeam = new Team(R_ShopTeam, true, false);
        Flush();
    }
    public void CancelTrans()
    {
        Player = null;
        R_Player = null;
        R_ShopTeam = null;
        ShopTeam = null;
        Hide();
    }

    public void Flush()
    {
        UpdataTable();
        UpdataRanksText();
    }

    public void InitPlayerUnitAdjust()
    {
        ButtonSelect_0 = 0;
        ButtonSelect_1 = 1;

        m_ToggleButton_0.onClick.AddListener(() =>
        {
            SetButtonSelect(ref ButtonSelect_0, ref ButtonSelect_1);
            UpdataTable();
        });

        m_ToggleButton_1.onClick.AddListener(() =>
        {
            SetButtonSelect(ref ButtonSelect_1, ref ButtonSelect_0);
            UpdataTable();
        });
    }

    public void InitShopUnitAdjust()
    {
        ButtonSelect_0 = -1;
        ButtonSelect_1 = 0;

        m_ToggleButton_0.onClick.AddListener(() =>
        {
            UpdataTable();
        });

        m_ToggleButton_1.onClick.AddListener(() =>
        {
            SetButtonSelect(ref ButtonSelect_1, ref ButtonSelect_0);
            UpdataTable();
        });
    }

    void SetButtonSelect(ref int ButtonSelect_0, ref int ButtonSelect_1)
    {
        ButtonSelect_0++;
        ButtonSelect_0 = ButtonSelect_0 > 2 ? 0 : ButtonSelect_0;
        ButtonSelect_0 = ButtonSelect_0 == ButtonSelect_1 ? ButtonSelect_0 + 1 : ButtonSelect_0;
        ButtonSelect_0 = ButtonSelect_0 > 2 ? 0 : ButtonSelect_0;
    }

    int ButtonSelect_0 = 0;
    int ButtonSelect_1 = 1;

    public void UpdataTable()
    {
        m_TablePage_0.table.ClreanTable(false);
        m_TablePage_1.table.ClreanTable(false);

        if (ShopTeam == null && ButtonSelect_0 != -1)
        {
            m_TablePage_0.ShowUnitList(
               new List<Unit>(Player.army.team[ButtonSelect_0].units),
               this,
               (Unit) => { PutOutUnitToTeam(Unit, Player.army.team[ButtonSelect_1]); }
           );

            m_TablePage_1.ShowUnitList(
                new List<Unit>(Player.army.team[ButtonSelect_1].units),
                this,
                (Unit) => { PutOutUnitToTeam(Unit, Player.army.team[ButtonSelect_0]); }
            );

            m_ToggleButton_0.GetComponentInChildren<Text>().text = "第 " + (ButtonSelect_0 + 1) + " 队";
        }
        else
        {
            m_TablePage_0.ShowUnitList(
               new List<Unit>(ShopTeam.units),
               this,
               (Unit) => { BuyUnit(Unit); },
               "买入"
           );

            m_TablePage_1.ShowUnitList(
                new List<Unit>(Player.army.team[ButtonSelect_1].units),
                this,
                (Unit) => { ShellUnit(Unit); },
                "卖出"
            );
            m_ToggleButton_0.GetComponentInChildren<Text>().text = "雇佣兵大厅";
        }

        m_ToggleButton_1.GetComponentInChildren<Text>().text = "第 " + (ButtonSelect_1 + 1) + " 队";

        m_Money.text = "拥有金币：" + Player.Money;
    }

    public void UpdataRanksText()
    {
        BattleTools.SetRankText(Ranks[0], Player.army.team[0], (Rank, Team) => { });
        BattleTools.SetRankText(Ranks[1], Player.army.team[1], (Rank, Team) => { });
        BattleTools.SetRankText(Ranks[2], Player.army.team[2], (Rank, Team) => { });
    }

    public void ShowInfoWindow(string info)
    {
        m_InfoWindow.SetActive(true);
        m_InfoWindow_Text.text = info;
    }

    public void CloseInfoWindow()
    {
        m_InfoWindow.SetActive(false);
        m_InfoWindow_Text.text = "";
    }

    void BuyUnit(Unit unit)
    {
        Debug.Log("Buy Unit Use 200$"); //
        if (Player.Money < unit.Setting.Price)
        {
            Debug.Log("Need Money " + unit.Setting.Price);
            return;
        }
        Player.Money -= unit.Setting.Price;
        Puting(unit, Player.army.team[ButtonSelect_1]);
        UpdataTable();
        UpdataRanksText();
    }

    void ShellUnit(Unit unit)
    {
        Debug.Log("Shell Unit Get 200$"); //
        Player.Money += unit.Setting.Price;
        Puting(unit, ShopTeam);
        UpdataTable();
        UpdataRanksText();
    }

    void PutOutUnitToTeam(Unit unit, Team team)
    {
        Puting(unit, team);
        UpdataTable();
        UpdataRanksText();
    }

    void Puting(Unit unit, Team team)
    {
        Unit cUnit = team.GetUnit(unit.Setting.Name);
        if (cUnit == null)
        {
            cUnit = new Unit(unit);
            cUnit.Count = 0;
            team.AddUnit(cUnit, true);
            //Game.DynamicDataManager.AddGameData(cUnit);
        }

        unit.Count--;
        cUnit.Count++;
        if (unit.Count == 0)
        {
            unit.team.RemoveUnit(unit, true);
            //Game.DynamicDataManager.DelGameData(unit);
        }
    }

}
