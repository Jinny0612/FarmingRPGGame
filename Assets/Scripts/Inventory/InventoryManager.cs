
using System;
using System.Collections.Generic;
using UnityEngine;


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
    /// 物品容器
    /// </summary>
    [SerializeField]
    private SO_ItemList itemList = null;

    private void Start()
    {
        CreateItemDetailsDictionary();
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
}
