using System;
using System.Collections.Generic;

/// <summary>
/// ����ƶ�����¼�����  �����ƶ��¼���Ҫ���õĲ���
/// </summary>
/// <param name="inputx">x�����ƶ�</param>
/// <param name="inputy">y�����ƶ�</param>
/// <param name="isWalking">�Ƿ�����·</param>
/// <param name="isRunning">�Ƿ����ܲ�</param>
/// <param name="isIdle">�Ƿ���У�վ����</param>
/// <param name="isCarrying">�Ƿ��ֳֹ���</param>
/// <param name="toolEffect">����Ч��</param>
/// <param name="isUsingToolRight">ʹ�ù��ߵķ���</param>
/// <param name="isUsingToolLeft"></param>
/// <param name="isUsingToolUp"></param>
/// <param name="isUsingToolDown"></param>
/// <param name="isLiftingToolRight">���𹤾ߵķ���</param>
/// <param name="isLiftingToolLeft"></param>
/// <param name="isLiftingToolUp"></param>
/// <param name="isLiftingToolDown"></param>
/// <param name="isPickingRight">ʰȡ�ķ���</param>
/// <param name="isPickingLeft"></param>
/// <param name="isPickingUp"></param>
/// <param name="isPickingDown"></param>
/// <param name="isSwingingToolRight">�Ӷ����ߵķ���</param>
/// <param name="isSwingingToolLeft"></param>
/// <param name="isSwingingToolUp"></param>
/// <param name="isSwingingToolDown"></param>
/// <param name="idleUp">����վ���ķ���</param>
/// <param name="idleDown"></param>
/// <param name="idleLeft"></param>
/// <param name="idleRight"></param>
public delegate void MovementDelegate(float inputx, float inputy, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
                                        ToolEffect toolEffect,
                                        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
                                        bool isLiftingToolRight,bool isLiftingToolLeft,bool isLiftingToolUp,bool isLiftingToolDown,
                                        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
                                        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
                                        bool idleUp, bool idleDown, bool idleLeft, bool idleRight);



/// <summary>
/// �����¼�������
/// ������̬��ͨ�����ڶ���һЩȫ�ֵġ�ͨ�õĹ��ܻ��߹����࣬��Щ���ܿ��ܻᱻ����Ӧ�ó���ʹ��   �޷���ʵ����
/// </summary>
public static class EventHandler
{

    //�������¼�
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    /// <summary>
    /// �����������¼�
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="inventoryItemList"></param>
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation,List<InventoryItem> inventoryItemList)
    {
        if (InventoryUpdatedEvent != null)
        {
            InventoryUpdatedEvent(inventoryLocation, inventoryItemList);
        }
    }


    //����ƶ��¼�  ������
    public static event MovementDelegate MovementEvent;

    //�������ߵ��õ��ƶ��¼�,����ͬ����
    //�κ��봥�����¼��ķ����߶����Ե�������ƶ��¼�
    public static void CallMovementEvent(float inputx, float inputy, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
                                        ToolEffect toolEffect,
                                        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
                                        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
                                        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
                                        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
                                        bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
    {
        //����Ƿ��ж�����
        if(MovementEvent != null)
        {
            //�ж����ߣ��ô��ݵĲ��������ƶ��¼�
            MovementEvent(inputx, inputy, isWalking, isRunning, isIdle, isCarrying,
                toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                idleUp, idleDown, idleLeft, idleRight);
        }
    }

    #region ��Ϸʱ���¼�
    /// <summary>
    /// �ƽ���Ϸʱ�� ��
    /// </summary>
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    /// <summary>
    /// �����ƽ���Ϸʱ���¼� ����
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason,int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameMinuteEvent != null)
        {
            AdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
        }
    }
    /// <summary>
    /// �ƽ���Ϸʱ�� Сʱ
    /// </summary>
    public static event Action<int,Season,int,string,int,int,int> AdvanceGameHourEvent;
    /// <summary>
    /// �����ƽ���Ϸʱ���¼�  Сʱ
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameHourEvent != null)
        {
            AdvanceGameHourEvent(gameYear,gameSeason, gameDay, gameDayOfWeek,gameHour, gameMinute, gameSecond);
        }
    }

    /// <summary>
    /// �ƽ���Ϸʱ���¼� ��
    /// </summary>
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;
    /// <summary>
    /// �����ƽ���Ϸʱ���¼� ��
    /// </summary>
    /// <param name="gameYear"></param>
    /// <param name="gameSeason"></param>
    /// <param name="gameDay"></param>
    /// <param name="gameDayOfWeek"></param>
    /// <param name="gameHour"></param>
    /// <param name="gameMinute"></param>
    /// <param name="gameSecond"></param>
    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        if(AdvanceGameDayEvent != null)
        {
            AdvanceGameDayEvent(gameYear,gameSeason,gameDay, gameDayOfWeek, gameHour, gameMinute,gameSecond);
        }
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        if(AdvanceGameSeasonEvent != null)
        {
            AdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
        }
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        if (AdvanceGameYearEvent != null)
        {
            AdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
        }
    }

    #endregion


    #region ��Ϸ�����л��¼�

    /// <summary>
    /// �ڳ���ж�ص���֮ǰ���¼��¼�
    /// </summary>
    public static event Action BeforeSceneUnloadFadeOutEvent;

    /// <summary>
    /// ��������ж�ص����¼�
    /// </summary>
    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        if(BeforeSceneUnloadFadeOutEvent != null)
        {
            BeforeSceneUnloadFadeOutEvent();
        }
    }

    /// <summary>
    /// �ڳ���ж��ǰ���¼�
    /// </summary>
    public static event Action BeforeSceneUnloadEvent;

    /// <summary>
    /// ��������ж��ǰ�¼�
    /// </summary>
    public static void CallBeforeSceneUnoladEvent()
    {
        if(BeforeSceneUnloadEvent != null){
            BeforeSceneUnloadEvent();
        }
    }

    /// <summary>
    /// �������غ���¼�
    /// </summary>
    public static event Action AfterSceneLoadEvent;

    /// <summary>
    /// �����������غ���¼�
    /// </summary>
    public static void CallAfterSceneLoadEvent()
    {
        if(AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
    }

    /// <summary>
    /// ����������ʾ����¼�
    /// </summary>
    public static event Action AfterSceneLoadFadeInEvent;

    /// <summary>
    /// ��������������ʾ����¼�
    /// </summary>
    public static void CallAfterSceneLoadFadeInEvent()
    {
        if( AfterSceneLoadFadeInEvent != null)
        {
            AfterSceneLoadFadeInEvent();
        }
    }

    #endregion
}