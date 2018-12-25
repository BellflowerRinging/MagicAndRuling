using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationContorl : ActionList
{
    public Text m_ForNameText;
    public Text m_ContentStr;

    List<CommunicationData> Comms = new List<CommunicationData>();

    public void CommunicateWithKing(King king)
    {
        if (king.CommunicationEntry == null) throw new System.Exception("Not Communication");
        ShowCommunication(king, king.CommunicationEntry);
    }

    public void ShowCommunication(King ToKing, CommunicationData data)
    {
        Show();
        ClearnActon();

        Comms.Add(data);
        if (data.SkillOnclick != null)
            data.SkillOnclick.DoSkill(Game.Player, ToKing);
            
        if ("End" == data.ContentStr) { CloseCommunication(); }

        m_ForNameText.text = ToKing.Name;
        m_ContentStr.text = data.ContentStr;

        if (data.NextComm != null && data.NextComm.Count > 0)
        {
            foreach (var item in data.NextComm)
            {
                AddAction(item.Key, () =>
                {
                    ShowCommunication(ToKing, item.Value);
                });
                item.Value.LastCons = data;
            }
            if (data.CanBack && data.LastCons != null)
                AddAction("返回", () => { ShowCommunication(ToKing, data.LastCons); });
        }
        else
        {
            if (data.CanBack && data.LastCons != null)
                AddAction("返回", () => { ShowCommunication(ToKing, data.LastCons); });

            AddAction("结束", () => { CloseCommunication(); });
        }

    }

    public void ShowCommunication(City city, CommunicationData data)
    {
        Show();
        ClearnActon();

        Comms.Add(data);
        if (data.SkillOnclick != null)
            data.SkillOnclick.DoSkill(Game.Player, city);

        m_ForNameText.text = data.ForName;
        m_ContentStr.text = data.ContentStr;

        if (data.NextComm != null && data.NextComm.Count > 0)
        {
            foreach (var item in data.NextComm)
            {
                AddAction(item.Key, () =>
                {
                    ShowCommunication(city, item.Value);
                });
                item.Value.LastCons = data;
            }
            if (data.CanBack && data.LastCons != null)
                AddAction("返回", () => { ShowCommunication(city, data.LastCons); });
        }
        else
        {
            if (data.CanBack && data.LastCons != null)
                AddAction("返回", () => { ShowCommunication(city, data.LastCons); });

            AddAction("结束", () => { CloseCommunication(); });
        }
    }

    public void CloseCommunication()
    {
        Hide();
        foreach (var item in Comms)
        {
            item.LastCons = null;
        }
        Comms.Clear();
    }
}