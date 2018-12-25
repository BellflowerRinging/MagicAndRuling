using System;
using System.Collections.Generic;

public class GameTimeManager
{
    public DateTime time { get; set; }
    DynamicGameSetting GameSetting;
    GameTimeContorl contorl;
    public float Scale = 3.4f; //正常速度 1秒=1分钟 3.4=每7分钟一天
    public GameTimeManager()
    {

    }

    public void SetTimeContorl(GameTimeContorl contorl)
    {
        this.contorl = contorl;
        this.contorl.SetManager(this);
    }

    public void SetDynGameSetting(DynamicGameSetting setting)
    {
        GameSetting = setting;
        time = new DateTime((long)GameSetting.Time * 10000000 * 60);
    }

    List<EveryMinuteAction> EmList = new List<EveryMinuteAction>();

    public void UpdateMinute()
    {
        GameSetting.Time++; //以分钟为单位 1秒=游戏中1分钟
        Game.MapContorl.SaveMapUnitPostion();
       Game.DynamicDataManager.Save(); //每秒储存游戏
       int minute = GameSetting.Time;

        foreach (var item in EmList)
        {
            if (minute % item.timespan == item.remainder)
                item.Invoke();
        }


        foreach (var item in TimerList)
        {
            if (minute == item.timer)
                item.Invoke();
        }

    }

    public void CreatEvetySpanMinteAction(string name, int MinuteSpan, Action<EveryMinuteAction> action)
    {
        foreach (var item in EmList)
        {
            if (item.name == name) throw new Exception("Action Name:" + name + " already in ActionList");
        }
        EveryMinuteAction minuteAction = new EveryMinuteAction(name, MinuteSpan, (GameSetting.Time / 60) % MinuteSpan, action);
        EmList.Add(minuteAction);
    }

    public void CreatEvetySpanHoursAction(string name, int HoursSpan, Action<EveryMinuteAction> action)
    {
        CreatEvetySpanMinteAction(name, HoursSpan * 60, action);
    }

    public void CreatEvetySpanDayAction(string name, int DaySpan, Action<EveryMinuteAction> action)
    {
        CreatEvetySpanMinteAction(name, DaySpan * 60 * 24, action);
    }

    public void RomoveEverySpanAction(string name) //尚未测试
    {
        for (int i = 0; i < EmList.Count; i++)
        {
            if (EmList[i].name == name)
            {
                EmList.RemoveAt(i);
                i--;
            }
        }
    }

    public void RemoveEveryAcitonAll()
    {
        EmList = new List<EveryMinuteAction>();
    }

    List<TimerAction> TimerList = new List<TimerAction>();

    public void CreatTimerAction(string name, int timer, Action<TimerAction> action)
    {
        TimerAction timerAction = new TimerAction(name, timer, action);
        TimerList.Add(timerAction);
    }

    public void CreatTimerActionWaitForMinute(string name, int minute, Action<TimerAction> action)
    {
        if (minute <= 0) throw new Exception("minute=" + minute + "<=0");
        CreatTimerAction(name, GameSetting.Time + minute, action);
    }

    public void CreatTimerActionWaitForHours(string name, int hours, Action<TimerAction> action)
    {
        if (hours <= 0) throw new Exception("hours=" + hours + "<=0");
        CreatTimerAction(name, GameSetting.Time + hours * 60, action);
    }

    public void CreatTimerActionWaitForDay(string name, int day, Action<TimerAction> action)
    {
        if (day <= 0) throw new Exception("day=" + day + "<=0");
        CreatTimerAction(name, GameSetting.Time + day * 60 * 24, action);
    }

    public void RomoveTimerAction(string name) //尚未测试
    {
        for (int i = 0; i < TimerList.Count; i++)
        {
            if (TimerList[i].name == name)
            {
                TimerList.RemoveAt(i);
                i--;
            }
        }
    }

    public void RemoveTimerActionAll()
    {
        TimerList = new List<TimerAction>();
    }

}
public class EveryMinuteAction
{
    public readonly string name;
    public readonly Action<EveryMinuteAction> action;
    public int timespan { get; private set; }
    public int remainder { get; private set; }

    private EveryMinuteAction() { }

    public EveryMinuteAction(string name, int timespan, int remainder, Action<EveryMinuteAction> action)
    {
        this.name = name;
        this.action = action;
        this.timespan = timespan;
        this.remainder = remainder;
    }

    public void Invoke()
    {
        action.Invoke(this);
    }
}

public class TimerAction
{
    public readonly string name;
    public int timer;
    public Action<TimerAction> action;

    public TimerAction(string name, int timer, Action<TimerAction> action)
    {
        this.name = name;
        this.timer = timer;
        this.action = action;
    }

    public void Invoke()
    {
        action.Invoke(this);
    }
}