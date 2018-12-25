using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionList : MonoBehaviour
{
    public Button DefaultItemButton;
    public GameObject ButtonPenal;
    List<Button> ButtonList = new List<Button>();
    int index = 0;

    private void Start()
    {

    }

    public void Show()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
    public void Hide()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void AddAction(string name, Action action, bool aotuhide = false)
    {
        Button btn;
        if (index == 0) btn = DefaultItemButton;
        else
        {
            btn = GameObject.Instantiate(DefaultItemButton, ButtonPenal.transform);
            ButtonList.Add(btn);
        }
        btn.GetComponentInChildren<Text>().text = name;
        btn.onClick.AddListener(() =>
        {
            action();
            if (aotuhide)
                Hide();
        });
        index++;
    }

    public void AddAction(Dictionary<string, Action> ActionDic)
    {
        foreach (var item in ActionDic)
        {
            AddAction(item.Key, item.Value);
        }
    }

    public void SetAction(Dictionary<string, Action> ActionDic)
    {
        ClearnActon();
        AddAction(ActionDic);
    }


    public void ClearnActon()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            GameObject.Destroy(ButtonList[i].gameObject);
        }
        DefaultItemButton.onClick.RemoveAllListeners();
        ButtonList = new List<Button>();
        index = 0;
    }
}