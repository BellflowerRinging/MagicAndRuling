using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldUiIndex : MonoBehaviour
{

    public Text m_TittleKingText;
    public Text m_TittleText;
    public Text m_BattleRecordText;

    public Button m_Skill_1_Button;
    public Text m_Skill_1_Text;
    public Button m_Skill_2_Button;
    public Text m_Skill_2_Text;
    public Button m_Skill_3_Button;
    public Text m_Skill_3_Text;

    public GameObject m_InfoWindow;
    public BattleUnitTablePage m_tablePage;
    public Button m_ClosTableButton;


    public Text m_InfoWindow_Text;
    public Button m_ClosButton;
    public Button m_AttackButton;
    public Text m_AttackButton_Text;
    public Button m_EscapeButton;
    public Text m_EscapeButton_Text;

    public ScrollRect m_BattleRecordScrollRect;
    public ContentSizeFitter m_BattleRecordContentSizeFitter;
    public Rank[] ranks = new Rank[6];




    Team[] teams = new Team[6];
    public void Start()
    {
        m_ClosButton.onClick.AddListener(() =>
        {
            HideInfoWindow();
        });

        m_ClosTableButton.onClick.AddListener(() =>
        {
            HideTableWindow();
        });

    }

    public void SetRound(int Round)
    {
        m_TittleText.text = "第 " + Round + " 波";
    }

    public void AddEventText(string str)
    {
        m_BattleRecordText.text += str + "\n";
        //CancelInvoke();
        //m_BattleRecordScrollRect.gameObject.SetActive(false);
        Invoke("wait", Time.deltaTime * 3);
    }

    void Update()
    {
        m_BattleRecordContentSizeFitter.SetLayoutHorizontal();
    }

    public void ClearEventText()
    {
        m_BattleRecordText.text = "";
    }

    void wait()
    {
        m_BattleRecordScrollRect.verticalScrollbar.value = 0;
    }

    ///index 0-2 player 3-5 Enemy
    public void SetRankText(int index, Team team)
    {
        BattleTools.SetRankText(ranks[index], team, (Rank, Team) => { RegistTeamButton(Rank, Team); });
        // Rank rank = ranks[index];

        // rank.Hero_1_Text.text = "";
        // rank.Hero_2_Text.text = "";
        // rank.Army_Text_0.text = "";
        // rank.Army_Text_1.text = "";

        // if (team != null)
        // {
        //     if (team.MainHero != null) rank.Hero_1_Text.text = team.MainHero.Name;
        //     if (team.MinorHero != null) rank.Hero_2_Text.text = team.MinorHero.Name;
        //     TeamSummaryData dic = team.GetSummary();
        //     if (dic.Maxhp == 0) return;
        //     string str;
        //     str = string.Format("Hp:{0}/{1}\n攻击:{2}\n人数:{3}", dic.Hp, dic.Maxhp, dic.Ad, dic.Count);
        //     rank.Army_Text_0.text = str;
        //     str = string.Format("士气:{0}\n防御:{1}\n受伤:{2}", dic.Morale, dic.Ed, dic.Injury);
        //     rank.Army_Text_1.text = str;
        //     RegistTeamButton(rank, team);
        // }

        teams[index] = team;
    }

    public void RegistTeamButton(Rank rank, Team team)
    {
        rank.Hero_1_Button.onClick.AddListener(() =>
         {
             if (team == null) return;

             ShowHeroInfo(team.MainHero);
         });

        rank.Hero_2_Button.onClick.AddListener(() =>
        {
            if (team == null) return;

            ShowHeroInfo(team.MinorHero);
        });

        HideTableWindow();

        rank.Army_Button.onClick.AddListener(() =>
        {
            if (team == null) return;
            ShowTableWindow();
            m_tablePage.ShowUnitList(new List<Unit>(team.units), this);
        });
    }

    public void ShowInfoWindow(string info)
    {
        m_InfoWindow.SetActive(true);
        m_InfoWindow_Text.text = info;
    }
    public void HideInfoWindow()
    {
        m_InfoWindow.SetActive(false);
        m_InfoWindow_Text.text = "";
    }

    public void ShowTableWindow()
    {
        m_tablePage.gameObject.SetActive(true);
    }
    public void HideTableWindow()
    {
        m_tablePage.gameObject.SetActive(false);
    }


    void ShowHeroInfo(Hero hero)
    {
        if (hero == null) return;
        string fom = "{0}：\t{1}\n{2}";
        string BuffIntroduce = "";

        foreach (var item in hero.buff)
        {
            if (item != null) BuffIntroduce += item.Info + "\n";
        }

        ShowInfoWindow(string.Format(fom, hero.ShowName, hero.Introduce, BuffIntroduce));
    }
}

