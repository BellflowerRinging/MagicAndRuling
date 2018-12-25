using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class MapUnit : MonoBehaviour
{
    public Text m_NameText;
    public bool CanMove;
    public float Speed = 200;
    public string GameDataName;
    public string Name { get { return m_NameText.text; } set { m_NameText.text = value; } }
    public GameDataType UnitType { get; protected set; }
    public Vector2 TargetPostion { get; protected set; }
    public MapUnit TargetUnit { get; protected set; }
    public Vector2 Size { get; private set; }

    public MapContorl mapContorl;

    public MapContorl MapContorl
    {
        get { return mapContorl; }
        set
        {
            if (mapContorl == null) mapContorl = value;
        }
    }

    public void SetTargetPostion(Vector2 pos)
    {
        pos = new Vector2(
            Mathf.Clamp(pos.x, 100, 2900), //width
            Mathf.Clamp(pos.y, -1818, -100) 
        );
        TargetPostion = pos;
    }

    // Use this for initialization
    void Start()
    {
        TargetPostion = transform.localPosition;
        Size = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
    }

    public virtual void Initialization()
    {
    }

    public virtual void OnClickByMap()
    {
        Game.EventTextContorl.AddEventTextLn(Name + " OnClickByMap");
    }

    public virtual void OnClickByNearPanel()
    {
        Game.EventTextContorl.AddEventTextLn(Name + " OnClickByNearPanel");
    }

    public void MoveToUnit(MapUnit unit)
    {
        Vector3 pos = transform.localPosition - unit.transform.localPosition;
        pos.Normalize();
        pos = pos * 60 + unit.transform.localPosition;
        SetTargetPostion(pos);
        TargetUnit = unit;
    }

    public void ArriveUnit()
    {
        Game.EventTextContorl.AddEventTextLn(Name + " arrive " + TargetUnit.Name);
        TargetUnit = null;
    }

    protected T ByGameData<T>(GameData data, GameDataType t2) where T : GameData
    {
        if (data.DataType != t2)
            throw new System.Exception("data type:" + System.Enum.GetName(typeof(GameDataType), data) + " != " + System.Enum.GetName(typeof(GameDataType), t2));
        return (T)data;
    }


}
