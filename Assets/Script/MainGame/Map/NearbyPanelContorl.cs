using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NearbyPanelContorl : ActionList
{
    public Button m_PlayerArmyEditButton;
    public Button m_PlayerAttriButton;
    public Button m_DiaryButton;
    public MapContorl m_MapContorl;
    public List<MapUnit> NearByPlayer { get; private set; }

    void Start()
    {
        m_PlayerArmyEditButton.onClick.AddListener(() =>
        {
            Game.UnitTranPanelContorl.Show(Game.Player);
        });

        NearByPlayer = new List<MapUnit>();
    }
    void Update()
    {
        for (int i = 0; i < m_MapContorl.m_AllUnits.Count; i++)
        {
            MapUnit unit = m_MapContorl.m_AllUnits[i];
            Vector2 locPos = unit.transform.localPosition;
            Vector2 tarPos = unit.TargetPostion;
            float speed = Time.deltaTime * unit.Speed;
            if (tarPos == locPos || speed == 0 || !unit.CanMove) continue;

            unit.transform.localPosition = Vector2.MoveTowards(locPos, tarPos, speed);

            if (unit.TargetUnit != null)
            {
                if (Vector3.Distance(unit.transform.localPosition, unit.TargetUnit.transform.localPosition) <= 62f)
                {
                    unit.ArriveUnit();
                }
            }
        }

        List<MapUnit> olist = new List<MapUnit>();

        foreach (var item in m_MapContorl.m_AllUnits)
        {
            if (Vector3.Distance(m_MapContorl.PlayerUnit.transform.localPosition, item.transform.localPosition) <= 62f)
            {
                if (item != m_MapContorl.PlayerUnit)
                    olist.Add(item);
            }
        }

        bool isin = false;

        foreach (var item in NearByPlayer)
        {
            if (!olist.Contains(item))
            {
                isin = true;
                break;
            }
        }
        if (olist.Count != NearByPlayer.Count) isin = true;

        if (isin)
        {
            NearByPlayer.Clear();
            NearByPlayer = olist;
            ToggleNearByPenal();
        }
        //
    }

    public void ToggleNearByPenal()
    {
        if (gameObject.activeSelf)
            if (NearByPlayer.Count > 0)
            {
                ClearnActon();
                foreach (var item in NearByPlayer)
                {
                    AddAction(item.Name, () => { item.OnClickByNearPanel(); });
                }
            }
            else
            {
                //NearByPeanl.Hide();
                ClearnActon();
                AddAction("无", () => { });
            }
    }
}
