using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTablePage : MonoBehaviour
{
    public TableObj table;
    // Use this for initialization
    public ScrollRect scroolrect;
    bool IsInit = false;
    void Awake()
    {
        //table = GetComponent<TableObj>();
        //table.SetTittleRow(new string[] { "兵种", "生命", "攻击", "防御", "数量", "操作" });
        //scroolrect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    void Init()
    {
        if (IsInit) return;
        //table = GetComponent<TableObj>();
        table.SetTittleRow(new string[] { "兵种", "生命", "攻击", "防御", "价格", "数量", "操作" });
        //scroolrect = GetComponent<ScrollRect>();
        IsInit = true;
    }


    public void ShowUnitList(List<Unit> list, UnitTranPanelContorl contorl, Action<Unit> GetOutUnitToTeam, string strc = "给出")
    {
        if (IsInit)
            table.ClreanTable(false);
        Init();

        TableCell[] row = new TableCell[7];

        foreach (var item in list)
        {

            if (item == null) continue;
            //row[0] = table.CreateInputFieldTableCell(item.Setting.Name);
            row[0] = table.CreateTableButtonCell(item.Setting.Name, (cell) => { contorl.ShowInfoWindow(item.Setting.Name + ":\n" + item.Setting.Introduce); });
            row[1] = table.CreateInputFieldTableCell(item.Setting.HP + "");
            row[2] = table.CreateInputFieldTableCell(item.Setting.AD + "");
            row[3] = table.CreateInputFieldTableCell(item.Setting.DE + "");
            row[4] = table.CreateInputFieldTableCell(item.Setting.Price + "");
            row[5] = table.CreateInputFieldTableCell(item.Count + "");
            for (int i = 1; i < 5; i++)
            {
                ((TableInputFieldCell)row[i]).SetRacastTarget(false);
            }
            //row[3] = table.CreateTableButtonCell("详情", (cell) => { Debug.Log(cell.IndexRow + " " + cell.IndexColumn); });
            row[6] = table.CreateTableButtonCell(strc, (cell) => { GetOutUnitToTeam(item); });
            table.AddRow(row);
        }

        if (table.m_TableRows.Count == 0)
        {
            row[0] = table.CreateInputFieldTableCell("无");
            row[1] = table.CreateInputFieldTableCell("");
            row[2] = table.CreateInputFieldTableCell("");
            row[3] = table.CreateInputFieldTableCell("");
            row[4] = table.CreateInputFieldTableCell("");
            row[5] = table.CreateInputFieldTableCell("");
            row[6] = table.CreateInputFieldTableCell("");
            for (int i = 0; i < 6; i++) ((TableInputFieldCell)row[i]).SetRacastTarget(false);
            table.AddRow(row);
        }

        scroolrect.verticalNormalizedPosition = 1;
    }


}
