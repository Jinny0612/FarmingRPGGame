using System;
using System.Collections.Generic;

/// <summary>
/// 玩家移动相关事件代理
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
}