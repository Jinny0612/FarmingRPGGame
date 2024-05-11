using System;
using System.Collections.Generic;

/// <summary>
/// 玩家移动相关事件代理  定义移动事件需要设置的参数
/// </summary>
/// <param name="inputx">x方向移动</param>
/// <param name="inputy">y方向移动</param>
/// <param name="isWalking">是否在走路</param>
/// <param name="isRunning">是否在跑步</param>
/// <param name="isIdle">是否空闲（站立）</param>
/// <param name="isCarrying">是否手持工具</param>
/// <param name="toolEffect">工具效果</param>
/// <param name="isUsingToolRight">使用工具的方向</param>
/// <param name="isUsingToolLeft"></param>
/// <param name="isUsingToolUp"></param>
/// <param name="isUsingToolDown"></param>
/// <param name="isLiftingToolRight">举起工具的方向</param>
/// <param name="isLiftingToolLeft"></param>
/// <param name="isLiftingToolUp"></param>
/// <param name="isLiftingToolDown"></param>
/// <param name="isPickingRight">拾取的方向</param>
/// <param name="isPickingLeft"></param>
/// <param name="isPickingUp"></param>
/// <param name="isPickingDown"></param>
/// <param name="isSwingingToolRight">挥动工具的方向</param>
/// <param name="isSwingingToolLeft"></param>
/// <param name="isSwingingToolUp"></param>
/// <param name="isSwingingToolDown"></param>
/// <param name="idleUp">空闲站立的方向</param>
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
/// 公共事件处理类
/// 公共静态类通常用于定义一些全局的、通用的功能或者工具类，这些功能可能会被整个应用程序使用   无法被实例化
/// </summary>
public static class EventHandler
{

    //库存更新事件
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    /// <summary>
    /// 发布库存更新事件
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


    //玩家移动事件  订阅者
    public static event MovementDelegate MovementEvent;

    //被发布者调用的移动事件,参数同上面
    //任何想触发此事件的发布者都可以调用这个移动事件
    public static void CallMovementEvent(float inputx, float inputy, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
                                        ToolEffect toolEffect,
                                        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
                                        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
                                        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
                                        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
                                        bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
    {
        //检查是否有订阅者
        if(MovementEvent != null)
        {
            //有订阅者，用传递的参数触发移动事件
            MovementEvent(inputx, inputy, isWalking, isRunning, isIdle, isCarrying,
                toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                idleUp, idleDown, idleLeft, idleRight);
        }
    }

    #region 游戏时间事件
    /// <summary>
    /// 推进游戏时间 分
    /// </summary>
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    /// <summary>
    /// 发布推进游戏时间事件 分钟
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
    /// 推进游戏时间 小时
    /// </summary>
    public static event Action<int,Season,int,string,int,int,int> AdvanceGameHourEvent;
    /// <summary>
    /// 发布推进游戏时间事件  小时
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
    /// 推进游戏时间事件 天
    /// </summary>
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;
    /// <summary>
    /// 发布推进游戏时间事件 天
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


    #region 游戏场景切换事件

    /// <summary>
    /// 在场景卸载淡出之前的事件事件
    /// </summary>
    public static event Action BeforeSceneUnloadFadeOutEvent;

    /// <summary>
    /// 发布场景卸载淡出事件
    /// </summary>
    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        if(BeforeSceneUnloadFadeOutEvent != null)
        {
            BeforeSceneUnloadFadeOutEvent();
        }
    }

    /// <summary>
    /// 在场景卸载前的事件
    /// </summary>
    public static event Action BeforeSceneUnloadEvent;

    /// <summary>
    /// 发布场景卸载前事件
    /// </summary>
    public static void CallBeforeSceneUnoladEvent()
    {
        if(BeforeSceneUnloadEvent != null){
            BeforeSceneUnloadEvent();
        }
    }

    /// <summary>
    /// 场景加载后的事件
    /// </summary>
    public static event Action AfterSceneLoadEvent;

    /// <summary>
    /// 发布场景加载后的事件
    /// </summary>
    public static void CallAfterSceneLoadEvent()
    {
        if(AfterSceneLoadEvent != null)
        {
            AfterSceneLoadEvent();
        }
    }

    /// <summary>
    /// 场景加载显示后的事件
    /// </summary>
    public static event Action AfterSceneLoadFadeInEvent;

    /// <summary>
    /// 发布场景加载显示后的事件
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