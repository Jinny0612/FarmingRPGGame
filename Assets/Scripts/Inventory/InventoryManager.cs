
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������
/// ����
/// </summary>
public class InventoryManager : SingletonMonoBehvior<InventoryManager>
{
    /// <summary>
    /// �����ֵ�
    /// </summary>
    private Dictionary<int,ItemDetails> itemDetailsDictionary;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [SerializeField]
    private SO_ItemList itemList = null;

    private void Start()
    {
        CreateItemDetailsDictionary();
    }

    /// <summary>
    /// �����ݴ�scriptableObject ItemListд���ֵ�
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
    /// ��ȡ��Ʒ����
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
