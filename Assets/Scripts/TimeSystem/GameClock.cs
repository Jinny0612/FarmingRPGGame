using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 游戏时间显示
/// </summary>
public class GameClock : MonoBehaviour
{
    /// <summary>
    /// 时间
    /// </summary>
    [SerializeField] private TextMeshProUGUI timeText = null;
    /// <summary>
    /// 日期
    /// </summary>
    [SerializeField] private TextMeshProUGUI dateText = null;
    /// <summary>
    /// 季节
    /// </summary>
    [SerializeField] private TextMeshProUGUI seasonText = null;
    /// <summary>
    /// 年份
    /// </summary>
    [SerializeField] private TextMeshProUGUI yearText = null;


    private void OnEnable()
    {
        //订阅时间管理事件
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    private void OnDisable()
    {
        //取消订阅时间管理事件
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    /// <summary>
    /// 更新游戏时间
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    private void UpdateGameTime(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        //不会每秒都显示，只显示10分钟的增量
        gameMinute = gameMinute - (gameMinute % 10);

        string ampm = "";
        string minute;

        
        if (gameHour >= 12)
        {
            //下午
            ampm = " pm";
        }
        else
        {
            //上午
            ampm = " am";
        }

        if(gameHour >= 13)
        {
            gameHour -= 12;
        }
        if(gameMinute < 10)
        {
            minute = "0" + gameMinute.ToString();

        }
        else
        {
            minute = gameMinute.ToString();
        }

        string time = gameHour.ToString() + " : " + minute + ampm; 
        //设置UI显示内容
        timeText.SetText(time);
        dateText.SetText(gameDayOfWeek + ". " + gameDay.ToString());
        seasonText.SetText(gameSeason.ToString());
        yearText.SetText("Year " + gameYear);
    }
}
