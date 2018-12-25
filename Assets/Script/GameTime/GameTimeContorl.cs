using UnityEngine;
using UnityEngine.UI;

public class GameTimeContorl : MonoBehaviour
{
    public GameTimeManager manager;
    public Text m_text;

    public void SetManager(GameTimeManager manager)
    {
        this.manager = manager;
    }

    float MinuteCount = 0f;
    void Update()
    {
        if (manager == null) return;
        manager.time = manager.time.AddMinutes(Time.deltaTime * manager.Scale);

        MinuteCount += Time.deltaTime * manager.Scale;
        /*if (MinuteCount >= 1)
        {
            MinuteCount = MinuteCount - 1;
            manager.UpdateMinute();
            m_text.text = string.Format("{0}年,{1}月,{2}日\n{3}:{4}", manager.time.Year, manager.time.Month, manager.time.Day, manager.time.Hour, manager.time.Minute);
        }*/

        while (MinuteCount >= 1)
        {
            MinuteCount = MinuteCount - 1;
            manager.UpdateMinute();
            //m_text.text = string.Format("{0}年,{1}月,{2}日\n{3}:{4}", manager.time.Year, manager.time.Month, manager.time.Day, manager.time.Hour, manager.time.Minute);
            m_text.text = manager.time.ToString("yyyy-MM-dd\nHH:mm");
        }


        //Debug.Log(manager.time.Minute);
    }
}