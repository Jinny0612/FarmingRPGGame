using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ��Ϸʱ����ʾ
/// </summary>
public class GameClock : MonoBehaviour
{
    /// <summary>
    /// ʱ��
    /// </summary>
    [SerializeField] private TextMeshProUGUI timeText = null;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private TextMeshProUGUI dateText = null;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private TextMeshProUGUI seasonText = null;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField] private TextMeshProUGUI yearText = null;


    private void OnEnable()
    {
        //����ʱ������¼�
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }

    private void OnDisable()
    {
        //ȡ������ʱ������¼�
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;
    }

    /// <summary>
    /// ������Ϸʱ��
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
        //����ÿ�붼��ʾ��ֻ��ʾ10���ӵ�����
        gameMinute = gameMinute - (gameMinute % 10);

        string ampm = "";
        string minute;

        
        if (gameHour >= 12)
        {
            //����
            ampm = " pm";
        }
        else
        {
            //����
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
        //����UI��ʾ����
        timeText.SetText(time);
        dateText.SetText(gameDayOfWeek + ". " + gameDay.ToString());
        seasonText.SetText(gameSeason.ToString());
        yearText.SetText("Year " + gameYear);
    }
}
