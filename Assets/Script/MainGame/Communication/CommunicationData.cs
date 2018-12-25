using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationData : GameData
{
    public string ForName;
    public string ContentStr;
    public bool CanBack;
    public Skill SkillOnclick;
    public Dictionary<string, CommunicationData> NextComm;
    public CommunicationData LastCons;

    int MaxNextComm = 10;

    public CommunicationData(string name, bool UserSoure = true) : base(GameDataType.ConversationData, name)
    {
        Init(UserSoure);
    }
    public CommunicationData(GameData data, bool init = true, bool UserSoure = true) : base(data)
    {
        if (init)
            Init(UserSoure);
    }

    public override void Init(bool UserSoure)
    {
        //if (Content.ContainsKey("ForName")) { ForName = Content["ForName"]; } else { SaveValue("ForName", ""); };
        //if (Content.ContainsKey("ContentStr")) { ContentStr = Content["ContentStr"]; } else { SaveValue("ContentStr", ""); };

        // if (Content.ContainsKey("CanBack"))
        // {
        //     if (Content["CanBack"] == "0") CanBack = false;
        //     else CanBack = true;
        // }
        // else { SaveValue("CanBack", "1"); };

        // if (Content.ContainsKey("SkillOnclick"))
        //     SkillOnclick = GameDataTool.GetOrNewGameDataByDynamic<Skill>(UserSoure, Content["SkillOnclick"]);
        // else { SaveValue("SkillOnclick", ""); }

        Name = Content["0"];
        ForName = InitStringValue("ForName");
        ContentStr = InitStringValue("ContentStr");
        CanBack = InitBooltValue("CanBack", true);
        SkillOnclick = InitParaByDynamic<Skill>(UserSoure, "SkillOnclick");

        NextComm = new Dictionary<string, CommunicationData>();
        for (int i = 0; i < MaxNextComm; i++)
        {
            string name = "";
            if (Content.ContainsKey("NextCommName_" + i) && !string.IsNullOrEmpty(Content["NextCommName_" + i]))
            {
                name = Content["NextCommName_" + i];
                //避免循环
                if (NextComm.ContainsKey(name)) throw new System.Exception("NextComm.ContainsKey(name) name=" + name + " i=" + i);
            }
            else
            {
                SaveValue("NextCommName_" + i, "");
            }

            if (Content.ContainsKey("NextComm_" + i) && !string.IsNullOrEmpty(Content["NextComm_" + i]) && !string.IsNullOrEmpty(name))
            {
                NextComm.Add(name, GameDataTool.GetOrNewGameDataByStatic<CommunicationData>(true, Content["NextComm_" + i]));
            }
            else
            {
                SaveValue("NextComm_" + i, "");
            }
        }
    }
    public override void SaveToContent() { }
}
