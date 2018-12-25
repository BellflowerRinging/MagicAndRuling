using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitTablePage : MonoBehaviour
{
    public TableObj table;
    // Use this for initialization
    public ScrollRect scroolrect;
    bool IsInit = false;
    void Awake()
    {
        Init();
    }

    void Update()
    {

    }
    void Init()
    {
        if (IsInit) return;
        table.SetTittleRow(new string[] { "兵种", "生命", "攻击", "防御", "数量", "操作" });
        IsInit = true;
    }

    public void ShowUnitList(List<Unit> list, BattlefieldUiIndex uiIndex)
    {
        if (IsInit)
            table.ClreanTable(false);
        Init();
        TableCell[] row = new TableCell[6];
        foreach (var item in list)
        {
            if (item == null) continue;
            //row[0] = table.CreateInputFieldTableCell(item.Setting.Name);
            UnitSummaryData data = item.GetSummary();
            row[0] = table.CreateTableButtonCell(data.name, (cell) => { uiIndex.ShowInfoWindow(data.name + ":\n" + item.Setting.Introduce); });
            row[1] = table.CreateInputFieldTableCell(data.Hp + "");
            row[2] = table.CreateInputFieldTableCell(data.Ad + "");
            row[3] = table.CreateInputFieldTableCell(data.Ed + "");
            row[4] = table.CreateInputFieldTableCell(data.Count + "/" + data.AllCount + "");
            row[5] = table.CreateInputFieldTableCell(data.Morale + "");
            for (int i = 1; i < 6; i++)
            {
                ((TableInputFieldCell)row[i]).SetRacastTarget(false);
            }
            //row[3] = table.CreateTableButtonCell("详情", (cell) => { Debug.Log(cell.IndexRow + " " + cell.IndexColumn); });
            //row[4] = table.CreateTableButtonCell("给出", (cell) => { Debug.Log(cell.IndexRow + " " + cell.IndexColumn); });
            table.AddRow(row);
        }
        scroolrect.verticalNormalizedPosition = 1;

    }
}
