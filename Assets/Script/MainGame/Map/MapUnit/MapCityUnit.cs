using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCityUnit : MapUnit
{
    public City city;
    override public void Initialization()
    {
        if (!string.IsNullOrEmpty(GameDataName))
        {
            city = Game.DynamicDataManager.GetGameData<City>(GameDataName);
            m_NameText.text = city.CityName;
        }
        UnitType = GameDataType.City;
        CanMove = false;
    }

    override public void OnClickByMap()
    {
        MapContorl.MenuContorl.ClearnActon();
        MapContorl.MenuContorl.AddAction("前往", () => { MapContorl.PlayerUnit.MoveToUnit(this); }, true);
        MapContorl.MenuContorl.Show(transform.position);
    }

    override public void OnClickByNearPanel()
    {
        Game.CityCommunicationContorl.ShowCity(city);
    }
}
