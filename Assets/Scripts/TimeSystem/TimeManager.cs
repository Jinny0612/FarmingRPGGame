using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʱ�����
/// </summary>
public class TimeManager : SingletonMonoBehvior<TimeManager>
{
    #region ��Ϸʱ������
    /// <summary>
    /// ��
    /// </summary>
    private int gameYear = 1;
    /// <summary>
    /// ����
    /// </summary>
    private Season gameSeason = Season.Spring;
    /// <summary>
    /// ��
    /// </summary>
    private int gameDay = 1;
    /// <summary>
    /// Сʱ
    /// </summary>
    private int gameHour = 6;
    /// <summary>
    /// ����
    /// </summary>
    private int gameMinute = 30;
    /// <summary>
    /// ��
    /// </summary>
    private int gameSecond = 0;
    /// <summary>
    /// һ�ܵĵڼ���
    /// </summary>
    private string gameDayOfWeek = "Mon";
    /// <summary>
    /// ��Ϸʱ���Ƿ���ͣ
    /// </summary>
    private bool gameClockPaused = false;
    /// <summary>
    /// ��ʵʱ������ţ����ڼ�����Ϸʱ��
    /// </summary>
    private float gameTick = 0f;
    #endregion

    private void Start()
    {
        //��ʼ�������뿪ʼ�����¼�
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
    /// ʱ������
    /// </summary>
    private void GameTick()
    {
        gameTick += Time.deltaTime;
        //ʱ��ﵽ��Ϸ������һ����
        if(gameTick >= Settings.secondsPerGameSecond)
        {
            //������Ϸ������
            gameTick -= Settings.secondsPerGameSecond;
            UpdateGameSecond();
        }
    }

    /// <summary>
    /// ��Ϸʱ����������
    /// </summary>
    private void UpdateGameSecond()
    {
        //������Ϸ����
        gameSecond++;

        if(gameSecond > 59)
        {
            //�ﵽ��Ϸ60s��������Ϸ����������������
            gameSecond = 0;
            gameMinute++;

            if(gameMinute > 59)
            {
                //�ﵽ��Ϸ60���ӣ�������ϷСʱ������������
                gameMinute = 0;
                gameHour++;

                if(gameHour > 23)
                {
                    //�ﵽ��Ϸ24ʱ������������Сʱ������
                    gameHour = 0;
                    gameDay++;

                    if(gameDay > 30)
                    {
                        //�ﵽһ���£�������һ������������
                        gameDay = 1;

                        int gs = (int)gameSeason;
                        gs++;
                        gameSeason = (Season)gs;

                        if(gs > 3)
                        {
                            //����������������ã���һ��
                            gs = 0;
                            gameSeason = (Season)gs;
                            gameYear++;
                            if(gameYear > 9999)
                            {
                                gameYear = 1;
                            }
                            //������ݸ����¼�
                            EventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                        }
                        //�������ڸ����¼�
                        EventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                    }
                    gameDayOfWeek = GetDayOfWeek();
                    //�������ڸ����¼�
                    EventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);

                }
                //����Сʱ�����¼�
                EventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
            //�������Ӹ����¼�
            EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            Debug.Log("Game Year: " + gameYear + "  Game Season: " + gameSeason + " Game Day: " + gameDay + 
                " Game Hour: " + gameHour + " Game Minute: " + gameMinute);
        }
    }

    /// <summary>
    /// ��ȡ���ڶ�Ӧ������
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

    //����ʱ��仯���������Է���
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
