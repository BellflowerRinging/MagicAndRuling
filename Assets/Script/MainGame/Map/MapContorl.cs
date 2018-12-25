using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapContorl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    public RectTransform m_ContentTransfrom;
    private bool IsDrap = false;
    public List<MapUnit> m_AllUnits;
    public MapUnit m_DefaultKingUnit;
    public MapUnit m_DefaultCityUnit;
    public MapKingUnit PlayerUnit;
    public MapMenuContorl MenuContorl;
    public NearbyPanelContorl NearByPeanl;
    public Vector2 MapSize;

    public void Init()
    {
        m_AllUnits = new List<MapUnit>(GetComponentsInChildren<MapUnit>());

        foreach (var item in m_AllUnits)
        {
            item.MapContorl = this;
            item.Initialization();
        }

        foreach (var item in Game.DynamicDataManager.GetGameDatas())
        {
            if (item.DataType == GameDataType.King)
            {
                King king = (King)item;
                CreateKingMapUnit(king, true);
            }
        }

        foreach (var item in m_AllUnits)
        {
            if (item.GameDataName == "Player")
                PlayerUnit = (MapKingUnit)item;
        }

        // PlayerUnit = GetComponentInChildren<MapKingUnit>();
        MapSize = new Vector2(m_ContentTransfrom.rect.width, m_ContentTransfrom.rect.height);
        Debug.Log(MapSize);
    }

    public MapKingUnit CreateKingMapUnit(King king, bool toAllUnits = true)
    {
        MapKingUnit unit = GameObject.Instantiate<MapKingUnit>((MapKingUnit)m_DefaultKingUnit, m_ContentTransfrom);
        unit.Initialization();
        Vector2 vector = GetVector2ByString(king.MapUnitPostion);
        unit.transform.localPosition = new Vector3(vector.x, vector.y, 0);
        unit.SetKing(king);
        unit.gameObject.SetActive(true);
        unit.gameObject.name = king.Name;
        if (toAllUnits) m_AllUnits.Add(unit);
        return unit;
    }



    public void SaveMapUnitPostion()
    {
        //保存所有单位位置
        foreach (var item in m_AllUnits)
        {
            if (item.UnitType == GameDataType.King)
            {
                ((MapKingUnit)item).SavePostion();
            }

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        IsDrap = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsDrap) { IsDrap = false; return; }

        Vector2 mousePosition = Input.mousePosition;

        //Vector2 offset = new Vector2(m_ContentTransfrom.localPosition.x + m_ContentTransfrom.rect.width / 2, m_ContentTransfrom.localPosition.y + m_ContentTransfrom.rect.height / 2); ;

        Vector2 offset = new Vector2(m_ContentTransfrom.localPosition.x, m_ContentTransfrom.localPosition.y + m_ContentTransfrom.rect.height - 150);

        mousePosition -= offset;

        MapUnit OnMouseClickUnit = null;

        foreach (var item in m_AllUnits)
        {
            Vector2 locPos = item.transform.localPosition;
            if (mousePosition.y > locPos.y && locPos.y < locPos.y + item.Size.y && mousePosition.x > locPos.x - item.Size.x / 2 && mousePosition.x < locPos.x + item.Size.x / 2)
            {
                OnMouseClickUnit = item;
                OnMouseClickUnit.OnClickByMap();
                // Game.EventTextContorl.AddEventTextLn(item.m_NameText.text + " On Click");
                break;
            }
        }

        //如果两个物体层叠怎么办----------------------------

        if (OnMouseClickUnit == null)
        {
            PlayerUnit.SetTargetPostion(mousePosition);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void ShowNearbyPanel()
    {
        NearByPeanl.Show();
    }

    public void HideNearbyPanel()
    {
        NearByPeanl.Hide();
    }

    static public Vector2 GetVector2ByString(string str)
    {
        if (string.IsNullOrEmpty(str)) return Vector2.zero;
        MatchCollection match = Regex.Matches(str, @"[^\(]+(?=,)|[^,]+(?=\))");
        return new Vector2(float.Parse(match[0].Value), float.Parse(match[1].Value));
    }
}
