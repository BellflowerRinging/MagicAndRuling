using UnityEngine;
using UnityEngine.UI;

public class CityCommunicationContorl : MonoBehaviour
{
    public Text m_CityName;
    public Text m_CityIntroduce;
    public Text m_CityBuffIntroduce;
    public Button m_LocationNameButton;
    public ActionList NpcActionList;
    public City loc_city;
    public CommunicationContorl NpcCommunicationContorl;
    public CommunicationContorl ForceCommContorl;

    void Start()
    {
        m_LocationNameButton.onClick.AddListener(() => { NpcCommunicationContorl.Hide(); });
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        NpcActionList.ClearnActon();
        gameObject.SetActive(false);
    }

    public void Leave()
    {
        loc_city = null;
        Hide();
    }

    public void ShowCity(City city)
    {
        this.loc_city = city;
        m_CityName.text = city.CityName;
        m_CityIntroduce.text = city.Introduce;
        m_CityBuffIntroduce.text = city.BuffIntroduce;

        foreach (var item in city.CommunicationEntrys)
        {
            if (item == null) continue;
            NpcActionList.AddAction(item.ForName, () => { NpcCommunicationContorl.ShowCommunication(city, item); });

            //   if (item.Name == "CityCommEntry_UnitShop")
            //   {
            //   }
        }
        NpcActionList.AddAction("离开", () => { Leave(); });
        Show();
    }

    void Update()
    {
        if (loc_city != null)
        {
            if (!isNearLocCity())
            {
                Leave();
            }
        }
    }

    bool isNearLocCity()
    {
        foreach (var item in Game.MapContorl.NearByPeanl.NearByPlayer)
        {
            if (item.UnitType == GameDataType.City)
            {
                if (((MapCityUnit)item).city == loc_city)
                    return true;
            }
        }
        return false;
    }
}