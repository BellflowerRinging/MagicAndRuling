using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameEventTextContorl : MonoBehaviour
{
    public Text m_Content;

    public ScrollRect m_ScrollRect;

    public void AddEventText(string text)
    {
        m_Content.text += text;
        m_ScrollRect.verticalScrollbar.value = 0;
    }

    public void AddEventTextLn(string text)
    {
        AddEventText(text + "\n");
    }
}
