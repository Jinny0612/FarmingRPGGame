
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


/// <summary>
/// 库存管理
/// 单例
/// </summary>
public class InventoryManager : SingletonMonoBehvior<InventoryManager>
{
    /// <summary>
    /// 详情字典
    /// </summary>
    private Dictionary<int,ItemDetails> itemDetailsDictionary;

    /// <summary>
    /// 库存列表
    /// 数组每个元素都是一个list
    /// </summary>
    public List<InventoryItem>[] inventoryLists;

    /// <summary>
    /// 不同库存类型的容量
    /// 数组中下标为库存类型枚举值，对应的元素值为库存容量
    /// </summary>
    [HideInInspector] public int[] inventoryListCapacityIntArry;

    /// <summary>
    /// 物品容器
    /// </summary>
    [SerializeField]
    private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();
        //创建库存信息
        CreateInventoryLists();

        //物品信息在awake中初始化，确保使用时在awake后使用
        CreateItemDetailsDictionary();
    }

    /// <summary>
    /// 所有类型库存信息
    /// </summary>
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for(int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityIntArry = new int[(int)InventoryLocation.count];

        //初始化玩家背包空间容量
        inventoryListCapacityIntArry[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// 将数据从scriptableObject ItemList写入字典
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary  = new Dictionary<int,ItemDetails>();

        foreach(ItemDetails itemDetials in itemList.itemDetials)
        {
            itemDetailsDictionary.Add(itemDetials.itemCode, itemDetials);
        }
    }

    /// <summary>
    /// 将物品添加到库存对应位置后，删除场景中的这个物品
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="item"></param>
    /// <param name="gameobjectToDelete"></param>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameobjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameobjectToDelete);
    }

    /// <summary>
    /// 添加物品到库存中去
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation itemLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];

        //判断库存中是否已经有这个物品
        int itemPosition = FindItemInventory(itemLocation, item);

        if(itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        //调用库存更新事件
        EventHandler.CallInventoryUpdatedEvent(itemLocation, inventoryLists[(int)itemLocation]);
    }

    /// <summary>
    /// 增加物品在库存中的数量（已存在这个物品）
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem item = new InventoryItem();

        int quantity = inventoryList[itemPosition].itemQuantity + 1;
        item.itemQuantity = quantity;
        item.itemCode = itemCode;
        inventoryList[itemPosition] = item;
        //显示当前库存
        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// 增加物品在库存中的数量（不存在这个物品）
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);
        //显示当前库存
        //DebugPrintInventoryList(inventoryList);

    }

    /*private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach(InventoryItem item in inventoryList)
        {
            Debug.Log("Item Description:" + InventoryManager.Instance.GetItemDetails(item.itemCode).itemDescription + "    Item Quantity: " + item.itemQuantity);
        }
        Debug.Log("*************************************************************");
    }*/

    /// <summary>
    /// 查找库存中是否已有这个物品
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private int FindItemInventory(InventoryLocation itemLocation, Item item)
    {
        //当前的库存列表
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];

        for(int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == item.ItemCode)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 查找库存中是否已有这个物品
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private int FindItemInventory(InventoryLocation itemLocation, int itemCode)
    {
        //当前的库存列表
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];
        ItemDetails item = GetItemDetails(itemCode);
        if (item != null)
        {
            for (int i = 0; i < inventoryList.Count; i++)
            {
                if (inventoryList[i].itemCode == item.itemCode)
                {
                    return i;
                }
            }
        }
        
        return -1;
    }

    /// <summary>
    /// 获取物品详情
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetials;
        if(itemDetailsDictionary.TryGetValue(itemCode, out itemDetials)) 
        { 
            return itemDetials; 
        }
        return null;
    }

    /// <summary>
    /// 移除库存中的物品
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCode"></param>
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        //获取当前库存物品列表
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        //检测物品是否在库存中存在
        int itemPosition = FindItemInventory(inventoryLocation,itemCode);

        if(itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        //发布库存更新事件
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    /// <summary>
    /// 移除库存中当前位置的物品
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();
        //一次只移除一个物品
        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if(quantity > 0)
        {
            //还有剩余物品，修改数量
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            //没有剩余物品，删除这个位置的物体
            inventoryList.RemoveAt(itemPosition);
        }
    }
}
