using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间管理
/// </summary>
public class TimeManager : SingletonMonoBehvior<TimeManager>
{
    #region 游戏时间属性
    /// <summary>
    /// 年
    /// </summary>
    private int gameYear = 1;
    /// <summary>
    /// 季节
    /// </summary>
    private Season gameSeason = Season.Spring;
    /// <summary>
    /// 日
    /// </summary>
    private int gameDay = 1;
    /// <summary>
    /// 小时
    /// </summary>
    private int gameHour = 6;
    /// <summary>
    /// 分钟
    /// </summary>
    private int gameMinute = 30;
    /// <summary>
    /// 秒
    /// </summary>
    private int gameSecond = 0;
    /// <summary>
    /// 一周的第几天
    /// </summary>
    private string gameDayOfWeek = "Mon";
    /// <summary>
    /// 游戏时间是否暂停
    /// </summary>
    private bool gameClockPaused = false;
    /// <summary>
    /// 真实时间的流逝，用于计算游戏时间
    /// </summary>
    private float gameTick = 0f;
    #endregion

    private void Start()
    {
        //初始发布分针开始计算事件
        EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);

    }

    private void Update()
    {
        if(!gameClockPaused)
        {
            GameTick();
        }
    }

    /// <summary>
    /// 时间流逝
    /// </summary>
    private void GameTick()
    {
        gameTick += Time.deltaTime;
        //时间达到游戏内流逝一分钟
        if(gameTick >= Settings.secondsPerGameSecond)
        {
            //更新游戏内秒数
            gameTick -= Settings.secondsPerGameSecond;
            UpdateGameSecond();
        }
    }

    /// <summary>
    /// 游戏时间秒数更新
    /// </summary>
    private void UpdateGameSecond()
    {
        //更新游戏秒数
        gameSecond++;

        if(gameSecond > 59)
        {
            //达到游戏60s，更新游戏分钟数，秒数重置
            gameSecond = 0;
            gameMinute++;

            if(gameMinute > 59)
            {
                //达到游戏60分钟，更新游戏小时数，分钟重置
                gameMinute = 0;
                gameHour++;

                if(gameHour > 23)
                {
                    //达到游戏24时，更新天数，小时数重置
                    gameHour = 0;
                    gameDay++;

                    if(gameDay > 30)
                    {
                        //达到一个月，进入下一季，天数重置
                        gameDay = 1;

                        int gs = (int)gameSeason;
                        gs++;
                        gameSeason = (Season)gs;

                        if(gs > 3)
                        {
                            //冬天结束，季节重置，下一年
                            gs = 0;
                            gameSeason = (Season)gs;
                            gameYear++;
                            if(gameYear > 9999)
                            {
                                gameYear = 1;
                            }
                            //发布年份更新事件
                            EventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                        }
                        //发布季节更新事件
                        EventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                    }
                    gameDayOfWeek = GetDayOfWeek();
                    //发布日期更新事件
                    EventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);

                }
                //发布小时更新事件
                EventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
            //发布分钟更新事件
            EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            Debug.Log("Game Year: " + gameYear + "  Game Season: " + gameSeason + " Game Day: " + gameDay + 
                " Game Hour: " + gameHour + " Game Minute: " + gameMinute);
        }
    }

    /// <summary>
    /// 获取日期对应的星期
    /// </summary>
    /// <returns></returns>
    private string GetDayOfWeek()
    {
        int totalDay = (((int)gameSeason) * 30) + gameDay;
        int dayOfWeek = totalDay % 7;

        switch(dayOfWeek)
        {
            case 1:
                return "Mon";
                case 2:
                return "Tue";
                case 3:
                return "Wed";
                case 4:
                return "Thu";
                case 5:
                return "Fri";
                case 6:
                return "Sat";
                case 0:
                return "Sun";
            default:
                return "";
        }
    }

    //测试时间变化的两个测试方法
   /* public void TestAdvanceGameMinute()
    {
        for(int i=0; i < 60; i++)
        {
            UpdateGameSecond();
        }
    }

    public void TestAdvanceGameDay()
    {
        for(int i = 0; i < 86400; i++)
        {
            UpdateGameSecond();
        }
    }*/
}
