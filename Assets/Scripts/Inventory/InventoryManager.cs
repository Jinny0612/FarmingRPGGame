
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


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
    /// ����б�
    /// ����ÿ��Ԫ�ض���һ��list
    /// </summary>
    public List<InventoryItem>[] inventoryLists;

    /// <summary>
    /// ��ͬ������͵�����
    /// �������±�Ϊ�������ö��ֵ����Ӧ��Ԫ��ֵΪ�������
    /// </summary>
    [HideInInspector] public int[] inventoryListCapacityIntArry;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [SerializeField]
    private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();
        //���������Ϣ
        CreateInventoryLists();

        //��Ʒ��Ϣ��awake�г�ʼ����ȷ��ʹ��ʱ��awake��ʹ��
        CreateItemDetailsDictionary();
    }

    /// <summary>
    /// �������Ϳ����Ϣ
    /// </summary>
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for(int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityIntArry = new int[(int)InventoryLocation.count];

        //��ʼ����ұ����ռ�����
        inventoryListCapacityIntArry[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
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
    /// ����Ʒ��ӵ�����Ӧλ�ú�ɾ�������е������Ʒ
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
    /// �����Ʒ�������ȥ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation itemLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)itemLocation];

        //�жϿ�����Ƿ��Ѿ��������Ʒ
        int itemPosition = FindItemInventory(itemLocation, item);

        if(itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        //���ÿ������¼�
        EventHandler.CallInventoryUpdatedEvent(itemLocation, inventoryLists[(int)itemLocation]);
    }

    /// <summary>
    /// ������Ʒ�ڿ���е��������Ѵ��������Ʒ��
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
        //��ʾ��ǰ���
        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// ������Ʒ�ڿ���е������������������Ʒ��
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
        //��ʾ��ǰ���
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
    /// ���ҿ�����Ƿ����������Ʒ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private int FindItemInventory(InventoryLocation itemLocation, Item item)
    {
        //��ǰ�Ŀ���б�
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
    /// ���ҿ�����Ƿ����������Ʒ
    /// </summary>
    /// <param name="itemLocation"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    private int FindItemInventory(InventoryLocation itemLocation, int itemCode)
    {
        //��ǰ�Ŀ���б�
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

    /// <summary>
    /// �Ƴ�����е���Ʒ
    /// </summary>
    /// <param name="player"></param>
    /// <param name="itemCode"></param>
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        //��ȡ��ǰ�����Ʒ�б�
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        //�����Ʒ�Ƿ��ڿ���д���
        int itemPosition = FindItemInventory(inventoryLocation,itemCode);

        if(itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        //�����������¼�
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    /// <summary>
    /// �Ƴ�����е�ǰλ�õ���Ʒ
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();
        //һ��ֻ�Ƴ�һ����Ʒ
        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if(quantity > 0)
        {
            //����ʣ����Ʒ���޸�����
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            //û��ʣ����Ʒ��ɾ�����λ�õ�����
            inventoryList.RemoveAt(itemPosition);
        }
    }
}
