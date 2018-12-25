using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapKingUnit : MapUnit
{
    public King king { get; private set; }

    override public void Initialization()
    {
        // if (!string.IsNullOrEmpty(GameDataName))
        //     SetKing(Game.DynamicDataManager.GetGameData<King>(GameDataName));
        UnitType = GameDataType.King;
        CanMove = true;
    }

    public void SetKing(King king)
    {
        this.king = king;
        m_NameText.text = king.Name;
        GameDataName = king.Name;
    }

    public void SavePostion()
    {
        king.MapUnitPostion = string.Format("({0},{1})", transform.localPosition.x, transform.localPosition.y);
    }

    override public void OnClickByMap()
    {
        Game.EventTextContorl.AddEventTextLn(Name + " OnClick - override");
    }

    override public void OnClickByNearPanel()
    {
        Game.MustCommunicationContorl.CommunicateWithKing(king);
    }
}
